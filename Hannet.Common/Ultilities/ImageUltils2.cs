using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Common.Ultilities
{
    public static class ImageUltils2
    {
        public static byte[] ConvertToImageFromFile(this string path)
        {
            try
            {
                //string pa=@path;
                //Image img = Image.FromFile(path);
                //byte[] arr;
                //using (MemoryStream ms = new MemoryStream())
                //{
                //    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                //    arr = ms.ToArray();
                //}
                return File.ReadAllBytes(path);
            }
            catch (Exception)
            {
                return new byte[0];
            }
        }
    }
}
