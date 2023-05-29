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
        }
    }
}
