using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmailLoader.MailboxChecking
{
    public interface IMailboxEmailManager : IDisposable
    {
        Task<List<MailboxEmail>> GetUnread();
        Task Delete(string emailId);
    }

    public class MailboxEmailManager : IMailboxEmailManager
    {
        private readonly string host;
        private readonly int port;
        private readonly string login;
        private readonly string password;

        private ImapClient client;

        public MailboxEmailManager(string host, int port, string login, string password)
        {
            this.host = host;
            this.port = port;
            this.login = login;
            this.password = password;
        }

        public async Task<List<MailboxEmail>> GetUnread() 
        {
            var emails = new List<MailboxEmail>();

            var client = await GetClient();
            var inbox = client.Inbox;

            var toProcess = inbox.Search(SearchQuery.NotDeleted);
            foreach (var uid in toProcess)
            {
                var message = inbox.GetMessage(uid);
                var email = MessageToMailboxEmail(message, uid);
                emails.Add(email);
            }

            return emails;
        }

        public async Task Delete(string emailId) 
        {
            var client = await GetClient();
            var uid = UniqueId.Parse(emailId);
            await client.Inbox.SetFlagsAsync(uid, MessageFlags.Deleted, silent: true);
            await client.Inbox.ExpungeAsync();
        }

        public void Dispose() 
        {
            if (client != null)
            {
                if (client.IsConnected)
                {
                    client.Disconnect(true);
                }

                client.Dispose();
            }
        }

        private async Task<ImapClient> GetClient() 
        {
            var isConnected = client?.IsConnected ?? false;
            if (!isConnected)
            {
                client = new ImapClient();
                await client.ConnectAsync(host, port, useSsl: true);
                await client.AuthenticateAsync(login, password);
                await client.Inbox.OpenAsync(FolderAccess.ReadWrite);
            }

            return client;
        }

        private MailboxEmail MessageToMailboxEmail(MimeMessage message, UniqueId uid) 
        {
            // https://github.com/jstedfast/MailKit/blob/master/FAQ.md#q-how-can-i-save-attachments

            var attachments = new List<MailboxEmailAttachment>(3);
            foreach (var attachment in message.Attachments)
            {
                var fileName = attachment.ContentDisposition?.FileName ?? attachment.ContentType.Name;
                var fileData = GetAttachmentData(attachment);

                attachments.Add(new MailboxEmailAttachment 
                { 
                    Name = fileName, 
                    Data = fileData 
                });
            }

            var email = new MailboxEmail
            {
                EmailId = uid.ToString(),
                Recipient = message.To.Mailboxes.First().Address,
                Subject = message.Subject,
                Text = message.TextBody ?? message.HtmlBody ?? "",
                Attachments = attachments,
            };
            return email;
        }

        private byte[] GetAttachmentData(MimeEntity attachment) 
        {
            var attachmentStream = new MemoryStream();

            if (attachment is MessagePart asciiAttachment)
            {
                asciiAttachment.Message.WriteTo(attachmentStream);
            }
            else
            {
                var base64Attachment = attachment as MimePart;
                base64Attachment.Content.DecodeTo(attachmentStream);
            }

            return attachmentStream.ToArray();
        }
    }
}
