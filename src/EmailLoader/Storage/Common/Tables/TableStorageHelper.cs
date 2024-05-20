using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailLoader.Storage.Common.Tables
{
    public static class TableStorageHelper
    {
        public static CloudTable GetTableReference(string storageConnectionString, string tableName)
        {
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            return new CloudTableClient(storageAccount.TableStorageUri, storageAccount.Credentials)
                .GetTableReference(tableName);
        }

        public static async Task<List<T>> ExecuteQueryAsync<T>(
            this CloudTable table,
            TableQuery<T> query,
            Action<List<T>> onProgress = null) where T : ITableEntity, new()
        {

            var items = new List<T>();
            TableContinuationToken token = null;

            do
            {
                var segment = await table.ExecuteQuerySegmentedAsync(query, token);
                token = segment.ContinuationToken;
                items.AddRange(segment);
                onProgress?.Invoke(items);

            } while (token != null);

            return items;
        }
    }
}
