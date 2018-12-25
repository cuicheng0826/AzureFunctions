using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileFunctions.Helper
{
    public class FileShareHelper
    {
        private static string GetConnectionString(string accountKey)
        {
            return Environment.GetEnvironmentVariable($"{accountKey}_BlobConnectionString");
        }
        private static async Task<CloudFileShare> GetCloudFileShareContainer(string connectionString, string containerName)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            //CloudFileClient 类是 Windows Azure File Service 客户端的逻辑表示，我们需要使用它来配置和执行对 File Storage 的操作。
            var fileClient = storageAccount.CreateCloudFileClient();
            //CloudFileShare 表示一个 File Share 对象。
            var share = fileClient.GetShareReference(containerName);
            //如果不存在就创建 File Share。
            await share.CreateIfNotExistsAsync();
            return share;
        }
    }
}
