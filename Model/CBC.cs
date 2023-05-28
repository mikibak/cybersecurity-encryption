using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cybersecurity_encryption.Model
{
    internal class CBC : Encryption
    {
        public CBC(byte[] key, byte[] message, int blockSize) : base(key, message, blockSize) { }

        public override byte[] Encrypt()
        {
            //TODO implement CBC
            return Message;
        }
        public override byte[] Decrypt()
        {
            //TODO implement CBC
            return Message;
        }
    }
}
