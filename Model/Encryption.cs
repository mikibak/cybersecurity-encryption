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
        public byte[] IV { get; set; }
        public Encryption() {
        }
        virtual public void GenerateIV() { }
        public abstract byte[] Encrypt(byte[] key, byte[] plaintext);
        public abstract byte[] Decrypt(byte[] key, byte[] ciphertext);
        public abstract void Padding(byte[] byteArray, bool isEncrypting, byte[] cipherKey, byte[]? IV);
    }
}
