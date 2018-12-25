using System;
using System.Collections.Generic;
using System.Text;
using System.DrawingCore;
using System.DrawingCore.Drawing2D;

namespace FileFunctions
{
    public class ImageHelper
    {
        /// <summary>
        /// 改变图片片分辨力率
        /// </summary>
        /// <param name="bmp">推按内容</param>
        /// <param name="newW">新的宽度</param>
        /// <param name="newH">新的 高度</param>
        /// <param name="Mode"></param>
        /// <returns></returns>
        public static Bitmap KiResizeImage(Image originImage, int newWidth)
        {
            var oldWidth = originImage.Width;
            var oldHeight = originImage.Height;
            var newHeight = (int)(((double)newWidth / oldWidth) * oldHeight);
            var bitmap = new Bitmap(newWidth, newHeight);
            using (var graphic = Graphics.FromImage(bitmap))
            {
                //设置图片质量
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //图片内容填充
                graphic.DrawImage(originImage, new Rectangle(0, 0, newWidth, newHeight), new Rectangle(0, 0, oldWidth, oldHeight), GraphicsUnit.Pixel);
                graphic.Dispose();
            }
            return bitmap;
        }



    }
}
