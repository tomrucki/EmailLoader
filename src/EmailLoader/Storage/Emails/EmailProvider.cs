using EmailLoader.Storage.Common.Tables;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailLoader.Storage.Emails
{
    public interface IEmailProvider 
    {
        Task Add(EmailEntity email);
        Task<EmailEntity> GetEmail(string emailId);
    }

    public class EmailProvider : IEmailProvider
    {
        const string TableName = "email";
        private readonly string storageConnectionString;

        public EmailProvider(string storageConnectionString)
        {
            this.storageConnectionString = storageConnectionString;
        }

        public async Task Add(EmailEntity email) 
        {
            var table = TableStorageHelper.GetTableReference(storageConnectionString, TableName);
            await table.CreateIfNotExistsAsync();
            await table.ExecuteAsync(TableOperation.Insert(email));
        }

        public async Task<EmailEntity> GetEmail(string emailId) 
        {
            var table = TableStorageHelper.GetTableReference(storageConnectionString, TableName);
            var tableExists = await table.ExistsAsync();
            if (!tableExists)
            {
                throw new EmailProviderException($"Storage table does not exist");
            }

            var queryResult = await table.ExecuteAsync(TableOperation.Retrieve<EmailEntity>(emailId, EmailEntity.DefaultRowKey));
            if (queryResult.Result is null)
            {
                throw new EmailProviderException($"Email with id {emailId} not found");
            }

            var email = (EmailEntity)queryResult.Result;
            email.Attachments ??= new List<EmailEntityAttachment>();

            return email;
        }
    }
}
