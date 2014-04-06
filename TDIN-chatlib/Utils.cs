using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace TDIN_chatlib
{
    public static class Utils
    {

        public static string hashBytes(byte[] b)
        {
            if (b == null || b.Length == 0)
                return null;

            string hash = String.Empty;
            byte[] byteOut = null;

            using (var crypt = new SHA256Managed())
            {
                byteOut = crypt.ComputeHash(b);
            }

            foreach (byte bit in byteOut)
            {
                hash += bit.ToString("x2");
            }

            return hash;

        }

        public static string generateRandomHash()
        {
            byte[] bytes = new byte[16];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }

            return hashBytes(bytes);
        }
    }
}
