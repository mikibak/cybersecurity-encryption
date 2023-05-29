using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace cybersecurity_encryption.Model
{
    internal class CBC : Encryption
    {
        public CBC() {
            aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.None;
        }
        public override void setKey(byte[] key)
        {
            aes.Key = key;
            using(MD5 hash = MD5.Create())
            {
                aes.IV = hash.ComputeHash(key);
            }
        }
    }
}
