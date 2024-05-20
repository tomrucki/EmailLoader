using EmailLoader.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailLoader.Storage.Emails
{

    public class EmailStore : IEmailStore
    {
        private readonly IEmailAttachmentProvider emailAttachmentProvider;
        private readonly IEmailProvider emailProvider;

        public EmailStore(IEmailAttachmentProvider emailAttachmentProvider, IEmailProvider emailProvider)
        {
            this.emailAttachmentProvider = emailAttachmentProvider;
            this.emailProvider = emailProvider;
        }

        public async Task<string> Add(AddEmail add) 
        {
            var newId = Guid.NewGuid().ToString();

            var attachments = add.Attachments
                .Select(a => (a.Name, StorageName: Guid.NewGuid().ToString(), a.Data))
                .ToList();
            var entityAttachments = attachments
                .Select(a => new EmailEntityAttachment { Name = a.Name, StorageName = a.StorageName })
                .ToList();
            await emailProvider.Add(new EmailEntity(newId, add.Recipient, add.Subject, add.Text, entityAttachments));

            
            if (attachments.Any())
            {
                var dataAttachments = attachments
                    .Select(a => new EmailAttachment { Name = a.StorageName, Data = a.Data })
                    .ToList();
                await emailAttachmentProvider.Add(newId, dataAttachments);
            }

            return newId;
        }

        public async Task<Email> GetEmail(string emailId, bool includeAttachments = true) 
        {
            var emailEntity= await emailProvider.GetEmail(emailId);
            var email = new Email
            {
                EmailId = emailEntity.EmailId,
                Recipient = emailEntity.Recipient,
                Subject = emailEntity.Subject,
                Text = emailEntity.Text
            };

            if (includeAttachments && emailEntity.Attachments.Any())
            {
                var attachments = await emailAttachmentProvider.Get(emailId);
                email.Attachments = attachments
                    .Select(a => new Email.Attachment
                    {
                        Name = emailEntity.Attachments.First(ea => ea.StorageName == a.Name).Name,
                        Data = a.Data,
                    })
                    .ToList();
            }
            else
            {
                email.Attachments = new List<Email.Attachment>();
            }

            return email;
        }
    }
}
