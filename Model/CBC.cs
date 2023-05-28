using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cybersecurity_encryption.Model
{
    internal class CBC : Encryption
    {
        public CBC(){ }

        public override byte[] Encrypt(byte[] key, byte[] message)
        {
            //TODO implement CBC
            return message;
        }
        public override byte[] Decrypt(byte[] key, byte[] message)
        {
            //TODO implement CBC
            return message;
        }
    }
}
