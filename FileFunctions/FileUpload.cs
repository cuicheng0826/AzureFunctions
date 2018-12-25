
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using FileFunctions.Models;
using FileFunctions.Helper;

namespace FileFunctions
{
    public static class FileUploadFunctions
    {

        static RSAHelper rsaHeler = new RSAHelper(RSAHelper.RSAType.RSA2, System.Text.Encoding.UTF8, ConfigSetting.privateKey, ConfigSetting.publicKey);
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("FileUpload")]
        [RequestSizeLimit(100_000_000)]
        public async static Task<IActionResult> FileUpload([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
            try
            {
                var token = req.Form.Get<string>("token", validate: t => !string.IsNullOrEmpty(t));
                if (string.IsNullOrEmpty(token) || token.Trim() != ConfigSetting.TokenVal?.Trim())
                {
                    return new BadRequestObjectResult("签名验证错误！");
                }
                var files = req.Form.Files;
                if (files.Count == 0)
                    return new BadRequestObjectResult("没有文件");

                var accountKey = req.Form.Get<string>("accountKey", validate: t => !string.IsNullOrEmpty(t));
                var path = req.Form.Get<string>("path", validate: t => !string.IsNullOrEmpty(t));


                return new OkObjectResult(await files.UploadFilesAsync(accountKey, path));
            }
            catch (Exception ex)
            {
                log.Info("FileUpload:" + ex.Message + "_" + ex.StackTrace);
                return new BadRequestObjectResult(ex.Message);
            }
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("FileDownload")]
        public async static Task<IActionResult> FileDownload([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]FileDownloadModel req, TraceWriter log)
        {
            try
            {
                if (req.token != ConfigSetting.TokenVal)
                {
                    return new BadRequestObjectResult("token验证错误");
                }
                return new OkObjectResult(await DownloadLogic.DownloadZIPFilesAsync(req.filePath, req.accountKey, req.path));
            }
            catch (Exception ex)
            {

                log.Info("FileDownload:" + ex.Message + "_" + ex.StackTrace);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
