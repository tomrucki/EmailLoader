
using Azure.Storage.Blobs;
using System.Threading.Tasks;

namespace EmailLoader.Storage.Common.Blobs
{
    public static class BlobStorageHelper 
    {
        public static async Task<BlobContainerClient> GetBlobContainer(string storageConnectionString, string containerName) 
        {
            var container = new BlobContainerClient(storageConnectionString, containerName);
            await container.CreateIfNotExistsAsync();
            return container;
        }
    }
}
