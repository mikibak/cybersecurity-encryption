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
        public byte[] Key { get; set; }
        public byte[] Message { get; set; }
        public byte[] IV { get; set; }

        protected Aes aes;
        public void setKey(byte[] key)
        {
            aes.Key = key;
        }
        public byte[] getKey()
        {
            return aes.Key;
        }
        public void setIV(byte[] key)
        {
            aes.IV = key;
        }
        public byte[] getIV()
        {
            return aes.IV;
        }
        public Encryption() {
        }
        virtual public void GenerateIV() { }
        public byte[] Encrypt(byte[] plaintext)
        {
            using (var encryptor = aes.CreateEncryptor())
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(plaintext, 0, plaintext.Length);
                        cs.FlushFinalBlock();
                    }

                    return ms.ToArray();
                }
            }
        }
        public byte[] Decrypt(byte[] ciphertext)
        {
            using (var encryptor = aes.CreateDecryptor())
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(ciphertext, 0, ciphertext.Length);
                        cs.FlushFinalBlock();
                    }

                    return ms.ToArray();
                }
            }
        }
    }
}
