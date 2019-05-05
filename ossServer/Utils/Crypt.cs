using System.IO;
using System.Security.Cryptography;

namespace ossServer.Utils
{
    public class Crypt
    {
        private static string ToHex(byte[] b)
        {
            var result = "";

            for (var i = 0; i < b.Length; i++)
                result += b[i].ToString("X2");

            return result;
        }

        public static string MD5Hash(byte[] b)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var h = md5.ComputeHash(b);

            return ToHex(h);
        }

        public static string MD5Hash(string s)
        {
            var b = new byte[s.Length];

            for (var i = 0; i < s.Length; i++)
                b[i] = (byte)s[i];

            return MD5Hash(b);
        }

        public static string MD5Hash(FileStream fs)
        {
            fs.Seek(0, SeekOrigin.Begin);

            MD5 md5 = new MD5CryptoServiceProvider();
            var h = md5.ComputeHash(fs);

            return ToHex(h);
        }

        public static string MD5Hash(MemoryStream fs)
        {
            fs.Seek(0, SeekOrigin.Begin);

            MD5 md5 = new MD5CryptoServiceProvider();
            var h = md5.ComputeHash(fs);

            return ToHex(h);
        }

        public static string fMD5Hash(string Fajlnev)
        {
            FileStream fs = null;

            try
            {
                fs = new FileStream(Fajlnev, FileMode.Open, FileAccess.Read);
                return MD5Hash(fs);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
    }
}
