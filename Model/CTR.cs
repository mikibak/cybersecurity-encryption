using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace cybersecurity_encryption.Model
{
    internal class CTR : Encryption
    {
        public CTR()
        {
            aes = Aes.Create();
            //aes.Mode = CipherMode.;
            aes.Padding = PaddingMode.None;
        }
        public override void setKey(byte[] key)
        {
            aes.Key = key;
            using (MD5 hash = MD5.Create())
            {
                aes.IV = hash.ComputeHash(key);
            }
        }
    }
}
