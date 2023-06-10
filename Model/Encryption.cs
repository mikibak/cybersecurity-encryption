using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace cybersecurity_encryption.Model
{
    public abstract class Encryption
    {
        public byte[] Message { get; set; }

        protected byte[] IV;
        protected byte[] key;
        public virtual void setKey(byte[] key)
        {
            this.key = key;
        }
        public byte[] getKey()
        {
            return this.key;
        }
        public byte[] getIV()
        {
            return this.IV;
        }
        public void GenerateInitializationVector()
        {
            SecureRandom random = new SecureRandom();
            byte[] iv = new byte[this.key.Length];
            random.NextBytes(iv);
            this.IV = iv;
        }
        public abstract byte[] Encrypt(byte[] plaintext);
        public abstract byte[] Decrypt(byte[] ciphertext);
    }
}
