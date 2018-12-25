using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FileFunctions.Models;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;
using System.DrawingCore;
using System.DrawingCore.Imaging;

namespace FileFunctions
{
    public static class UploadLogic
    {
        /// <summary>
        /// 异步上传文件
        /// </summary>
        /// <param name="files">文件</param>
        /// <param name="accountKey">Azure链接字串</param>
        /// <param name="path">存储路径</param>
        /// <returns></returns>
        public static async Task<IEnumerable<string>> UploadFilesAsync(this IFormFileCollection files, string accountKey, string path)
        {
            path = BlobHelper.GetPath(path);
            var containerName = path;
            var blobName = DateTime.Now.ToString("yyyyMMdd");
            if (path.Contains("/"))
            {
                containerName = path.Split('/')[0];
                blobName = path.Substring(path.IndexOf('/') + 1) + "/" + blobName;
            }
            var container = await BlobHelper.GetBlobContainer(accountKey.GetConnectionString(), containerName);
            var urls = new List<string>();
            //多个文件上传
            foreach (var file in files)
            {
                var fileName = $"{blobName}/{BlobHelper.RandomName(file.FileName)}";
                urls.Add(await container.UploadFileAsBlob(file, fileName));
                var postfix = Path.GetExtension(file.FileName);
                ///判断文件是否是图片文件，如果是图片文件还需要生成其他分辨率的图片
                if (Array.IndexOf(ConfigSetting.PicturePostfix, postfix.ToLower()) > -1)
                {
                    var img = Image.FromStream(file.OpenReadStream(), true);
                    //要是上传的图片宽度大于生成其他分辨率的宽度的临界点时
                    if (img.Width > ConfigSetting.MinWidth)
                    {
                        //每个分辨率的图片上传一张
                        foreach (var item in ConfigSetting.PictureResolutions)
                        {
                            //压缩图片
                            using (var newBitmap = ImageHelper.KiResizeImage(img, item.Width))
                            {
                                //新的图片的名称
                                var imgBlobName = $"{blobName.Substring(0, blobName.LastIndexOf('.'))}-{item.Size}{Path.GetExtension(blobName)}";
                                using (var ms = new MemoryStream())
                                {
                                    newBitmap.Save(ms, ImageFormat.Jpeg);
                                    await container.UploadFileAsBlob(ms, imgBlobName);
                                }
                            }

                        }
                    }
                }
            }

            return urls;
        }


    }
}
