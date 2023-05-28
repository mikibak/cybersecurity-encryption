using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cybersecurity_encryption.Model
{
    public abstract class Encryption
    {
        public byte[] Key { get; set; }
        public byte[] Message { get; set; }
        public int BlockSize { get; set; }
        public Encryption(byte[] key, byte[] message, int blockSize) {
            Key = key;
            Message = message;
            BlockSize = blockSize;
        }

        public abstract byte[] Encrypt();
        public abstract byte[] Decrypt();
    }
}
