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

        public static string generateRandomHash()
        {
            byte[] bytes = new byte[16],
                   byteOut;
            string hash = String.Empty;

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }

            using (var crypt = new SHA256Managed())
            {
                byteOut = crypt.ComputeHash(bytes);
            }

            foreach (byte bit in byteOut)
            {
                hash += bit.ToString("x2");
            }

            return hash;
        }
    }
}
