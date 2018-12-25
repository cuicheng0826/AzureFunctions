using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileFunctions
{
    public static class JsonHelper
    {
        /// <summary>
        /// Json转String
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ConvertToJson(object data)
        {
            return JsonConvert.SerializeObject(data);
        }
        /// <summary>
        /// string转Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ConvertToObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// 转换为格式化的JSON文件
        /// </summary>
        /// <param name="data">需要转换的实体</param>
        /// <returns></returns>
        public static string ConvertJsonString(object data)
        {
            //格式化json字符串
            JsonSerializer serializer = new JsonSerializer();
            StringWriter textWriter = new StringWriter();
            JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
            {
                Formatting = Formatting.Indented,
                Indentation = 4,
                IndentChar = ' '
            };
            serializer.Serialize(jsonWriter, data);
            return textWriter.ToString();

        }
    }
}
