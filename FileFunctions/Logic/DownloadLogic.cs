using FileFunctions.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;

namespace FileFunctions
{
    public class DownloadLogic
    {
        /// <summary>
        /// 打包下载文件，返回压缩包地址
        /// </summary>
        /// <param name="filePaths">要下载的文件路径</param>
        /// <param name="accountKey">Azure连接字符串</param>
        /// <param name="path">压缩包的存储路径</param>
        /// <returns></returns>
        public static async Task<string> DownloadZIPFilesAsync(List<string> filePaths, string accountKey, string path)
        {
            try
            {
                path = BlobHelper.GetPath(path);
                var containerName = path;
                var blobName = DateTime.Now.ToString("yyyyMMdd");
                if (path.Contains("/"))
                {
                    containerName = path.Split('/')[0];
                    blobName = path.Substring(path.IndexOf('/') + 1) + "/" + blobName;
                }
                ///Azure连接串
                var container = await BlobHelper.GetBlobContainer(accountKey.GetConnectionString(), containerName);
                ///打包文件地址
                var zipPath = $"{blobName}/ZipFile/{Guid.NewGuid().ToString("N")}.zip";
                ///打包文件的Azure块
                var zipBlockBlob = container.GetBlockBlobReference(zipPath);

                using (var blobStream = await zipBlockBlob.OpenWriteAsync())
                {   ///读取压缩包的流
                    using (ZipOutputStream zipStream = new ZipOutputStream(blobStream))
                    {
                        ///循环要下载的文件地址
                        foreach (var file in filePaths)
                        {
                            ///获取排除https头的文件地址
                            var filePath = file.Substring(file.IndexOf(ConfigSetting.HttpHeader) + ConfigSetting.HttpHeader.Length + 1);
                            ///获取这个文件在Azure中的位置
                            var fileBlobName = filePath.Substring(filePath.IndexOf('/') + 1);
                            ///读取文件名（带后缀）
                            var fileName = file.Substring(file.LastIndexOf('/') + 1);
                            ///获取Azure中的文件
                            var fileBlockBlob = container.GetBlockBlobReference(fileBlobName);
                            using (var fileStream = await fileBlockBlob.OpenReadAsync())
                            {
                                ///文件流添加到压缩包流中
                                zipStream.PutNextEntry(new ZipEntry(fileName));
                                var buffer = new byte[fileStream.Length];
                                await fileStream.ReadAsync(buffer, 0, buffer.Length);
                                zipStream.Write(buffer, 0, buffer.Length);
                            }
                        }
                        //文件流写入
                        await zipStream.FlushAsync();
                    }
                }
                ///返回压缩包地址
                return zipBlockBlob?.Uri.ToString();
            }
            catch (Exception ex)
            {

                return "打包下载失败，" + ex.Message + ex.StackTrace;
            }
        }

    }
}
