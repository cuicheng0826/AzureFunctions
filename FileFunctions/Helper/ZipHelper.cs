using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace FileFunctions
{
    public class ZipHelper
    {
        #region 单例模式

        private volatile static ZipHelper _instance = null;
        private static readonly object lockHelper = new object();//线程锁
        /// <summary>
        /// 压缩、解压帮助类
        /// </summary>
        public static ZipHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockHelper)
                    {
                        if (_instance == null)
                        {
                            _instance = new ZipHelper();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region 构造函数
        public ZipHelper()
        {

        }
        #endregion

        #region 方法
        /// <summary>
        /// 简单压缩方法
        /// </summary>
        /// <param name="filePath">压缩内容保存路径</param>
        /// <param name="zipPath">压缩后文件保存路径</param>
        /// <returns></returns>
        public bool Compress(string filePath, string zipPath)
        {
            try
            {
                if (!Directory.Exists(filePath))
                    return false;
                if (!Directory.Exists(zipPath))
                    CreateDirectory(zipPath);
                ZipFile.CreateFromDirectory(filePath, zipPath);
            }
            catch (Exception ex)
            {
                LogHelper.LogMessage($"Compress({filePath},{zipPath}):{ ex.Message}");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 简单的解压方法
        /// </summary>
        /// <param name="zippath">压缩文件所在路径</param>
        /// <param name="savepath">解压后文件保存路径</param>
        /// <returns></returns>
        public bool DeCompress(string zippath, string savepath)
        {
            try
            {
                if (!Directory.Exists(zippath))
                    return false;
                ZipFile.ExtractToDirectory(zippath, savepath);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 指定目录下压缩指定类型文件
        /// </summary>
        /// <param name="filepath">指定目录</param>
        /// <param name="zippath">压缩后保存路径</param>
        /// <param name="folderName">压缩文件内部文件夹名</param>
        /// <param name="fileType">指定类型 格式如：*.dll</param>
        /// <returns></returns>
        public bool Compress(string filepath, string zippath, string folderName, string fileType)
        {
            try
            {
                IEnumerable<string> files = Directory.EnumerateFiles(filepath, fileType);
                using (ZipArchive zipArchive = ZipFile.Open(zippath, ZipArchiveMode.Create))
                {
                    foreach (var file in files)
                    {
                        var entryName = Path.Combine(folderName, Path.GetFileName(file));
                        zipArchive.CreateEntryFromFile(file, entryName);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 调用方法
        /// <summary>
        /// 创建父级路径
        /// </summary>
        /// <param name="infoPath"></param>
        private void CreateDirectory(string infoPath)
        {
            DirectoryInfo directoryInfo = Directory.GetParent(infoPath);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
        }
        #endregion
    }
}
