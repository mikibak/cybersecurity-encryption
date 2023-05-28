using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cybersecurity_encryption.Model
{
    internal class CTR : Encryption
    {
        public CTR() { }

        public override byte[] Encrypt(byte[] key, byte[] message)
        {
            //TODO implement CTR
            return message;
        }
        public override byte[] Decrypt(byte[] key, byte[] message)
        {
            //TODO implement CTR
            return message;
        }
        public override void Padding(byte[] byteArray, bool isEncrypting, byte[] cipherKey, byte[]? IV) { }
    }
}
