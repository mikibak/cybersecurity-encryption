using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cybersecurity_encryption.Model
{
    internal class CTR : Encryption
    {
        public CTR(byte[] key, byte[] message, int blockSize) : base(key, message, blockSize) { }

        public override byte[] Encrypt()
        {
            //TODO implement CTR
            return Message;
        }
        public override byte[] Decrypt()
        {
            //TODO implement CTR
            return Message;
        }
    }
}
