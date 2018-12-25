using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace FileFunctions
{
    public static class FileHelper
    {
        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="files">路径地址</param>
        /// <param name="blobName">下载存储地址</param>
        public static void SaveImg(List<string> files, string downloadPath)
        {
            try
            {
                WebClient mywebclient = new WebClient();
                foreach (var file in files)
                {
                    var fileName = file.Substring(file.LastIndexOf('/') + 1);
                    if (!Directory.Exists(downloadPath))
                        Directory.CreateDirectory(downloadPath);
                    var newfileName = $"{downloadPath}{fileName}";
                    if (!File.Exists(newfileName))
                        mywebclient.DownloadFile(file, newfileName);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogMessage($"SaveImg():{ ex.Message}");
                Console.WriteLine(ex.Message);
            }


        }

        /// <summary>
        /// 删除文件目录
        /// </summary>
        /// <param name="infoPath"></param>
        public static void DeleteDirectory(string infoPath)
        {
            DirectoryInfo directoryInfo = Directory.GetParent(infoPath);
            if (directoryInfo.Exists)
            {
                directoryInfo.Delete(true);
            }
        }
    }
}
