using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace KioskManagement.Common.Ultilities
{
    public static class ImageUltils
    {
        public static byte[] Base64ToJpeg(this byte[] base64)
        {
            if (base64 == null)
            {
                return null;
            }
#pragma warning disable CA1416 // Validate platform compatibility
            using (Image image = Image.FromStream(new MemoryStream(base64)))
#pragma warning restore CA1416 // Validate platform compatibility
            {
                using (var ms = new MemoryStream())
                {
#pragma warning disable CA1416 // Validate platform compatibility
                    image.Save(ms, ImageFormat.Jpeg);
#pragma warning restore CA1416 // Validate platform compatibility
                    return ms.ToArray();
                }
            }
        }

    }
}