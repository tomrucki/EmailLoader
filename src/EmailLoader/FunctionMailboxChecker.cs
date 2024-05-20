using EmailLoader.Storage.Emails;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EmailLoader.MailboxChecking
{
    public static class FunctionMailboxChecker
    {
        [FunctionName(nameof(RunMailboxChecker))]
        public static async Task RunMailboxChecker(
            [TimerTrigger("%MailboxCheckerSchedule%")]TimerInfo myTimer,
            ExecutionContext context,
            ILogger log)
        {
            log.LogInformation($"{nameof(RunMailboxChecker)} trigger function started at: {DateTime.UtcNow}");

            var config = ConfigHelper.Build(context);

            var checkerSettings = config.GetMailboxCheckerSettings();
            var mailboxManager = new MailboxEmailManager(checkerSettings.EmailServerDomain, checkerSettings.EmailServerPort, checkerSettings.EmailLogin, checkerSettings.EmailPassword);

            var storageConnectionString = config.GetStorageConnectionString();
            var emailProvider = new EmailProvider(storageConnectionString);
            var emailAttachmentProvider = new EmailAttachmentProvider(storageConnectionString);
            var emailStore = new EmailStore(emailAttachmentProvider, emailProvider);

            var emailProcessorQueue = new EmailProcessorQueue(storageConnectionString, Constants.EmailProcessorQueueName);

            try
            {
                await new MailboxChecker(mailboxManager, emailStore, emailProcessorQueue).Run();
            }
            catch (Exception ex)
            {
                log.LogError(ex, nameof(MailboxChecker) + " failed");
                throw;
            }
        }
    }
}
