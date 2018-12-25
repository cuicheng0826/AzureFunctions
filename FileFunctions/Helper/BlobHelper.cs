using FileFunctions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.DrawingCore;
using System.DrawingCore.Imaging;

namespace FileFunctions
{
    public static class BlobHelper
    {



        /// <summary>
        /// 异步上传图片
        /// </summary>
        /// <param name="url"></param>
        /// <param name="blobName"></param>
        /// <returns></returns>
        private static async Task<IEnumerable<string>> UploadResolutionsPicAsync(this CloudBlobContainer container, Stream stream, string blobName)
        {
            var urls = new List<string>();
            try
            {

                var img = Image.FromStream(stream, true);
                //要是上传的图片宽度大于生成其他分辨率的宽度的临界点时
                if (img.Width > ConfigSetting.MinWidth)
                {
                    //每个分辨率的图片上传一张
                    foreach (var item in ConfigSetting.PictureResolutions)
                    {
                        //压缩图片
                        var newBitmap = ImageHelper.KiResizeImage(img, item.Width);
                        //新的图片的名称
                        var imgBlobName = $"{blobName.Substring(0, blobName.LastIndexOf('.'))}-{item.Size}{Path.GetExtension(blobName)}";
                        using (var ms = new MemoryStream())
                        {
                            newBitmap.Save(ms, ImageFormat.Jpeg);
                            urls.Add(await container.UploadFileAsBlob(ms, imgBlobName));
                        }
                        newBitmap.Dispose();

                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }

            return urls;
        }



        #region 获取Azure的服务器连接字符串
        /// <summary>
        /// 获取Azure的服务器连接字符串
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="containerName">连接名字</param>
        /// <returns></returns>
        public static async Task<CloudBlobContainer> GetBlobContainer(string connectionString, string containerName)
        {
            try
            {
                var storageAccount = CloudStorageAccount.Parse(connectionString);
                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference(containerName.ToLower());
                await container.CreateIfNotExistsAsync();
                var blobPermissions = new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob };
                await container.SetPermissionsAsync(blobPermissions);

                return container;
            }
            catch (Exception ex)
            {
                LogHelper.LogMessage($"获取容器失败{ex.Message},{ connectionString},{ containerName}");
                throw new FuncException("获取容器失败," + connectionString + "," + containerName);
            }
        }
        #endregion

        #region 上传文件至Azure服务器


        /// <summary>
        /// 上传文件至Azure服务器
        /// </summary>
        /// <param name="container">链接字符串</param>
        /// <param name="file">文件</param>
        /// <param name="blobName">Azure块名称</param>
        /// <returns>服务器上地址</returns>
        public static async Task<string> UploadFileAsBlob(this CloudBlobContainer container, IFormFile file, string blobName)
        {
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var blockBlob = container.GetBlockBlobReference(blobName);
                    await blockBlob.UploadFromStreamAsync(stream);
                    return blockBlob?.Uri.ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogMessage($"上传文件失败{ex.Message}");
                throw new FuncException("上传文件失败");

            }
        }
        public static async Task<string> UploadFileAsBlob(this CloudBlobContainer container, Stream stream, string blobName)
        {
            try
            {
                var blockBlob = container.GetBlockBlobReference(blobName);
                //流的读取位置重置为0（重要）
                stream.Position = 0;
                await blockBlob.UploadFromStreamAsync(stream);
                return blockBlob?.Uri.ToString();
            }
            catch (Exception ex)
            {
                throw new FuncException("上传文件失败");

            }
        }
        #endregion

        #region 从Azure服务器上下载文件
        /// <summary>
        /// 从Azure服务器上下载文件
        /// </summary>
        /// <param name="container"></param>
        /// <param name="filePath"></param>
        /// <param name="blobName"></param>
        /// <returns></returns>
        private static async Task DownloadFileAsBlob(this CloudBlobContainer container, string newfilePath, string blobName)
        {
            try
            {
                ///获取文件的块（blobName）是要下载的文件地址
                var blockBlob = container.GetBlockBlobReference(blobName);

                ///下载文件
                await blockBlob.DownloadToFileAsync(newfilePath, FileMode.Create);
            }
            catch (Exception ex)
            {
                LogHelper.LogMessage($"下载文件失败{ex.Message}");
            }


        }

        /// <summary>
        /// 批量下载文件到指定文件夹
        /// </summary>
        /// <param name="containiner">连接字符串</param>
        /// <param name="files">要下载的文件地址列表</param>
        /// <param name="downloadPath">文件下载到的地址</param>
        /// <returns></returns>
        private static async Task DownloadFilesAsBlob(this CloudBlobContainer containiner, List<string> files, string downloadPath)
        {
            var httpHead = ConfigSetting.HttpHeader;
            foreach (var file in files)
            {
                ///文件名
                var fileName = file.Substring(file.LastIndexOf('/') + 1);
                ///要存储的文件地址（包括文件名）
                var newfilePath = $"{downloadPath}{fileName}";
                ///要下载的文件地址
                var blobName = file.Substring(httpHead.Length + 1);
                blobName = blobName.Substring(blobName.IndexOf('/') + 1);

                ///单个文件下载
                await DownloadFileAsBlob(containiner, newfilePath, blobName);
            }
        }
        #endregion

        #region 文件存储到本地 
        private static async Task SaveFile(Stream stream, string blobName)
        {
            var path = Path.GetFullPath(blobName);
            var bitmap = Image.FromStream(stream);
            bitmap.Save(path);
        }
        #endregion

        #region 随机生成文件名字
        /// <summary>
        /// 随机生成文件名字
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string RandomName(string filename)
        {
            return Guid.NewGuid().ToString("N") + Path.GetExtension(filename);
        }
        #endregion

        #region 路径处理（去掉首尾的“/”）
        /// <summary>
        /// 路径处理（去掉首尾的“/”）
        /// </summary>
        /// <param name="path">原始路径</param>
        /// <returns>处理后的路径</returns>
        public static string GetPath(string path)
        {
            if (path.IndexOf("/") == 0)
            {
                return path.Substring(1);
            }
            if (path.LastIndexOf("/") == path.Length - 1)
            {
                return path.Substring(0, path.Length - 1);
            }
            return path;
        }
        #endregion

        #region 删除Azure服务器上的文件
        /// <summary>
        /// 删除Azure服务器上的文件
        /// </summary>
        /// <param name="container"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static async Task DeleteFileAsync(this CloudBlobContainer container, string filePath)
        {
            try
            {
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(filePath);
                await blockBlob.DeleteAsync();
            }
            catch (Exception ex)
            {
                LogHelper.LogMessage($"删除Azure服务器上的文件{ex.Message}");
            }

        }
        #endregion

    }
}
