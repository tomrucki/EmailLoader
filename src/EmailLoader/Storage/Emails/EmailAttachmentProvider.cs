using EmailLoader.Storage.Common.Blobs;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EmailLoader.Storage.Emails
{
    public interface IEmailAttachmentProvider 
    {
        Task Add(string emailId, List<EmailAttachment> emailAttachments);
        Task<List<EmailAttachment>> Get(string emailId);
    }

    public class EmailAttachmentProvider : IEmailAttachmentProvider
    {
        const string containerName = "attachment";

        private readonly string storageConnectionString;

        public EmailAttachmentProvider(string storageConnectionString)
        {
            this.storageConnectionString = storageConnectionString;
        }

        public async Task Add(string emailId, List<EmailAttachment> emailAttachments) 
        {
            var container = await BlobStorageHelper.GetBlobContainer(storageConnectionString, containerName);

            var uploads = new List<Task>(emailAttachments.Count);
            foreach (var attaachment in emailAttachments)
            {
                var blobName = $"/{emailId}/{attaachment.Name}";
                var blob = container.GetBlobClient(blobName);
                uploads.Add(blob.UploadAsync(new MemoryStream(attaachment.Data)));
            }

            await Task.WhenAll(uploads);
        }

        public async Task<List<EmailAttachment>> Get(string emailId) 
        {
            var attachmentDirectory = $"/{emailId}/";
            var container = await BlobStorageHelper.GetBlobContainer(storageConnectionString, containerName);

            var blobList = container.GetBlobsAsync(prefix: attachmentDirectory);

            var attachments = new List<EmailAttachment>();
            await foreach (var blobListItem in blobList)
            {
                var blob = container.GetBlobClient(blobListItem.Name);
                var blobStream = new MemoryStream();
                await blob.DownloadToAsync(blobStream);

                attachments.Add(new EmailAttachment
                {
                    Name = blob.Name.Substring(attachmentDirectory.Length),
                    Data = blobStream.ToArray(),
                });
            }

            return attachments;
        }
    }

    public class EmailAttachment 
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}
