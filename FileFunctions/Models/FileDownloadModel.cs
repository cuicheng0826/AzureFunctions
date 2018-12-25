using System;
using System.Collections.Generic;
using System.Text;

namespace FileFunctions
{
    public class FileDownloadModel
    {
        public List<string> filePath { get; set; }
        public string accountKey { get; set; }
        public string path { get; set; }
        /// <summary>
        /// token
        /// </summary>
        public string token { get; set; }
    }
}
