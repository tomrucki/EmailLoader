using EmailLoader.Domain;
using EmailLoader.Storage.Emails;
using System.Linq;
using System.Threading.Tasks;

namespace EmailLoader.MailboxChecking
{
    public class MailboxChecker 
    {
        private readonly IMailboxEmailManager mailboxEmailManager;
        private readonly IEmailStore emailStore;
        private readonly IEmailProcessorQueue emailProcessorQueue;

        public MailboxChecker(
            IMailboxEmailManager mailboxEmailManager,
            IEmailStore emailStore,
            IEmailProcessorQueue emailProcessorQueue)
        {
            this.mailboxEmailManager = mailboxEmailManager;
            this.emailStore = emailStore;
            this.emailProcessorQueue = emailProcessorQueue;
        }

        public async Task Run() 
        {
            var unreadEmails = await mailboxEmailManager.GetUnread();
            foreach (var email in unreadEmails)
            {
                var addCommand = new AddEmail
                {
                    Recipient = email.Recipient,
                    Subject = email.Subject,
                    Text = email.Text,
                    Attachments = email.Attachments
                        .Select(a => new AddEmail.Attachment { Name = a.Name, Data = a.Data })
                        .ToList()
                };
                var storedEmailId = await emailStore.Add(addCommand);

                await emailProcessorQueue.Enqueue(storedEmailId);

                await mailboxEmailManager.Delete(email.EmailId);
            }
        }
    }
}
