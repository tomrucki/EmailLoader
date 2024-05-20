using Azure.Storage.Queues;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EmailLoader.Storage.Emails
{
    public class EmailProcessorQueue : IEmailProcessorQueue
    {
        private readonly string storageConnectionString;
        private readonly string queueName;

        public EmailProcessorQueue(string storageConnectionString, string queueName)
        {
            this.storageConnectionString = storageConnectionString;
            this.queueName = queueName;
        }

        public async Task Enqueue(string storedEmailId) 
        {
            var queue = new QueueClient(storageConnectionString, queueName);
            await queue.CreateIfNotExistsAsync();

            var encodedMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(storedEmailId));
            await queue.SendMessageAsync(encodedMessage);
        }
    }
}
