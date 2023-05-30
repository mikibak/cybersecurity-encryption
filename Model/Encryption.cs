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
            using (MD5 hash = MD5.Create())
            {
                IV = hash.ComputeHash(key);
            }
        }
        public byte[] getKey()
        {
            return this.key;
        }
        virtual public void GenerateIV() { }
        public abstract byte[] Encrypt(byte[] plaintext);
        public abstract byte[] Decrypt(byte[] ciphertext);
    }
}
