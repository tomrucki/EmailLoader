using EmailLoader.EmailProcessing;
using EmailLoader.ExternalApi;
using EmailLoader.Storage.Emails;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EmailLoader
{
    public static class FunctionEmailProcessor
    {
        [FunctionName(nameof(RunEmailProcessorFunction))]
        public static async Task RunEmailProcessorFunction(
            [QueueTrigger(Constants.EmailProcessorQueueName)] string emailId,
            ExecutionContext context,
            ILogger log)
        {
            log.LogInformation($"{nameof(RunEmailProcessorFunction)} trigger function started processing emailId {emailId} at: {DateTime.UtcNow}");

            var config = ConfigHelper.Build(context);

            var storageConnectionString = config.GetStorageConnectionString();
            var emailProvider = new EmailProvider(storageConnectionString);
            var emailAttachmentProvider = new EmailAttachmentProvider(storageConnectionString);
            var emailStore = new EmailStore(emailAttachmentProvider, emailProvider);

            var apiClientSettings = config.GetEmailProcessorSettings().GetExternalApiClientSettings();
            var apiClient = new ExternalApiClient(apiClientSettings);

            try
            {
                await new EmailProcessor(emailStore, apiClient).Process(emailId);
            }
            catch (EmailProcessorException ex) 
            {
                // don't throw, only log -> doesn't retry
                log.LogError(ex, nameof(EmailProcessor) + " failed");
            }
            catch (Exception ex)
            {
                log.LogError(ex, nameof(EmailProcessor) + " failed");
                throw;
            }
        }
    }
}
