using System;
using System.Collections.Generic;
using System.Text;

namespace FileFunctions.Models
{
    public static class ConfigSetting
    {
        /// <summary>
        /// 生成小图片的最小宽度临界点
        /// </summary>
        public static int MinWidth = 400;
        /// <summary>
        /// 图片文件的后缀
        /// </summary>
        public static string[] PicturePostfix = new string[] { ".jpg", ".jpeg", ".bmp", ".png", ".gif" };
        /// <summary>
        /// 图片分辨率列表
        /// </summary>
        public static List<PictureResolutionModel> PictureResolutions = GetPictureResolutions();

        /// <summary>
        /// RSA公钥
        /// </summary>
        public static string publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArb091KQFKukVedQVLgIF663SqJYB94QMXskAG9mW1iUBBisUorrF6t7wYQAt8ltBxwGsd4KjCY03FbuY/BNtX9sI0Y72Si3c2yK5x2ztPbfyK5umEPWaDKYusnKJl6hB2EONRkkRk2QocK1/XhnrvtdMWBKLc311sIcpt/dORKSGH8/L8BNcBrL0aKyc/gfuS/kBhs6DhDbtnFEVhXOAnIhS4AdlNsKzTX5Gok4Piuuc1TJltGILnF2K9nBST/lxM/om7bGpENsP3N6MGl+3kOMGlgOS+TNOZhgy02E4W+95iubIeDYmSfL9QE3hp9JsFuXI225/p4xaSNtodm23OwIDAQAB";
        /// <summary>
        /// RSA私钥
        /// </summary>
        public static string privateKey = "MIIEpAIBAAKCAQEArb091KQFKukVedQVLgIF663SqJYB94QMXskAG9mW1iUBBisUorrF6t7wYQAt8ltBxwGsd4KjCY03FbuY/BNtX9sI0Y72Si3c2yK5x2ztPbfyK5umEPWaDKYusnKJl6hB2EONRkkRk2QocK1/XhnrvtdMWBKLc311sIcpt/dORKSGH8/L8BNcBrL0aKyc/gfuS/kBhs6DhDbtnFEVhXOAnIhS4AdlNsKzTX5Gok4Piuuc1TJltGILnF2K9nBST/lxM/om7bGpENsP3N6MGl+3kOMGlgOS+TNOZhgy02E4W+95iubIeDYmSfL9QE3hp9JsFuXI225/p4xaSNtodm23OwIDAQABAoIBAQCTZ3K1Ha/wty7sXR7XGQpS6fhH5nWmvZcNODXqaxLJfz2+MQDiC9rtqdaCRfe1nu1Q3b+o6eJPsUsiGjby77ylTh3ORh/50a2HEpBSfb1O1ukVTIp56xaXUstdxUWtsSikRrPvBQFvbtPMvbbJbO6RzPLN6nX8N16JtjOUDLOQEiyFLHwNPgaqHtsLi7WMadorQYYPBrPE7t7yg10po60QpQ1SJkjhb4Z9668sVVXKFLXGX3sJNjp9JvAVH0w+zyu4A888kLw0isV3cjSSSTtsnhryyz3vFjm8QSWTk+4KGW38066Gtao9REiDcBlbQRIHK/oaZR2w8DtmIDynT515AoGBANSzOuvq1Abf3HZjEvNQmwPaDUU9NhvTVRKnM+KHRw5RNiYSO+5nP1qTbrEYbRa0OYPvOc8zDLPnb14E2L9EUTR1KIZgIdNv6GW7jVh5b3j+RWn+446iAlHwzGjBDLHmChffpZiQ0irTOM3WjjXWd7/dDwUnZY6fTFZWp1fTQPF9AoGBANEblQfUGmwX1d2ICzjB68FUhnjgdTschuqSnPo1/CDM/UtpKlzusvw3F737C2EpCrB8pIGsV+Y0nO+J+vqo20e4ezGe7uHF4JM82wr0PaKytUwIjftVYZeV3/69IljmBqQSf264UCXvxmj3GGuNdexFonAeijIcSF/2G/tshykXAoGBAJvtOIiygfT4L3pqbv4IXVuZgpj7oqsLZhZilNrAKQsJ5hRK0byX3A3efws8yNwYCwH5YfvPhMRuKWpXSz4MKyu1p5u65ZFHPKs3rwrpGxsr7F8kCFYL1n+Tc4Zn35Ka/VM7FP6fTDMw3TeozmARdYAl4lj45K6FgUTaCZuMttNRAoGAH/BQkQOKjZwOZGnVHPQxqDPa3jeacE7pJIcERwtb2T62KYCEofErkW5wx+nCUTLNmStQjSXfvx+mIMg4d3S3GXtKBcg6wG1S4epXopKvn1wgaN6doRfnchnIPsZwqpdnyEOzBxxL0Z+P9JRZPIQ80LvKFcn0XbcILvAOUha/hj0CgYB+CeZ7ItfLuBDTOeBJnw1i52AHnXsFHmszg2hB4wn8z0SOfhLV5ZlSiiG4a7JsCzD/4OiRBWRtFjvMUXi56KyIkm74hEfsPNPt4b4c9b0P+9I9XsNSXqxjDCJSVbQrPp2Bh7nn4Lz18I8PGt89KJzL0Oj3xhtPOpCUy0wmGwK63Q==";
        /// <summary>
        /// Token值（每个小时会更新）
        /// </summary>
        public static string TokenVal = TokenKeyVal();
        /// <summary>
        /// 图片Http地址
        /// </summary>
        public static string HttpHeader = "https://icewang.blob.core.chinacloudapi.cn";
        /// <summary>
        /// 获取图片分辨率列表
        /// </summary>
        /// <returns></returns>
        private static List<PictureResolutionModel> GetPictureResolutions()
        {
            var list = new List<PictureResolutionModel>();
            string resolution = Environment.GetEnvironmentVariable($"PictureResolution");
            if (!string.IsNullOrEmpty(resolution))
            {
                var resList = resolution.Split(',');
                foreach (var item in resList)
                {
                    var size = item.Split('|')[0];
                    var width = item.Split('|')[1];
                    list.Add(new PictureResolutionModel
                    {
                        Width = Convert.ToInt32(width),
                        Size = size
                    });
                }
            }
            return list;
        }

        private static string TokenKeyVal()
        {
            return Environment.GetEnvironmentVariable($"tokenKey")?.Trim();
        }

        /// <summary>
        /// 获取链接字符串的关键字
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public static string GetConnectionString(this string accountKey)
        {
            return Environment.GetEnvironmentVariable($"{accountKey}_BlobConnectionString");
        }
    }
}
