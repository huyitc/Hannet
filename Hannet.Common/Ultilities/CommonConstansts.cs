using System;
using System.Text;
using XSystem.Security.Cryptography;

namespace Hannet.Common.Ultilities
{
    public static class CommonConstansts
    {
        public enum TicketPriceSelected
        {
            LUOT = 0,
            KHUNGGIO = 1,
            BLOCK = 2,
            NGAY = 3
        }

        public enum TicketTypeSelected
        {
            THANG = 0, LUOT = 1
        }
    }

    public class AccountMD5
    {
        public string GetMD5(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

            StringBuilder sbHash = new StringBuilder();
            foreach (byte b in bHash)
            {
                sbHash.Append(String.Format("{0:x2}", b));
            }
            return sbHash.ToString();
        }
    }

    public class Common
    {
        public static string ConvertToString(object data)
        {
            if (data is null)
                return "";
            return data.ToString();
        }
    }
}