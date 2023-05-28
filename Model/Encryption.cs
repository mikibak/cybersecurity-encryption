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
        public Encryption() {
        }

        public abstract byte[] Encrypt(byte[] key, byte[] message);
        public abstract byte[] Decrypt(byte[] key, byte[] message);
    }
}
