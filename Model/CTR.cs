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
        }
    }
}
