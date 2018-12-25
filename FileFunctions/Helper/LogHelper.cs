using System;
using System.Collections.Generic;
using System.Text;

namespace FileFunctions
{
    public class LogHelper
    {
        public static void LogMessage(string message)
        {
            var url = "http://log.rplus.com/api/logs";
            var entity = new
            {
                LogTime = DateTime.Now,
                LogType = 2,
                ServerMode = 3,
                Message = message,
                Source = "FileFunctions",
            };
            var hander = new Dictionary<string, string>();
            var token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJFeHAiOjE4NTg5MDk1MzMuMCwiSWF0IjoxNTQzNTQ5NTMzLjAsIlVzZXJJZCI6MywiVXNlcm5hbWUiOiJsb2dnZXJAcnBsdXMuY29tIiwiSXNBZG1pbiI6ZmFsc2UsIklzRXhwaXJlZCI6ZmFsc2V9.rZaHwmzjKPu_IgzMER3_btiLbJK7a9_yPDVpjBB18m4";
            hander.Add("token", token);
            HttpHelper.PostJson(url, JsonHelper.ConvertJsonString(entity), hander);
        }
    }
}
