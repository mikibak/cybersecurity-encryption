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

        protected Aes aes;
        public virtual void setKey(byte[] key)
        {
            aes.Key = key;
            using (MD5 hash = MD5.Create())
            {
                aes.IV = hash.ComputeHash(key);
            }
        }
        public byte[] getKey()
        {
            return aes.Key;
        }
        public Encryption() {
        }
        virtual public void GenerateIV() { }
        public virtual byte[] Encrypt(byte[] plaintext)
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
        public virtual byte[] Decrypt(byte[] ciphertext)
        {
            using (var decryptor = aes.CreateDecryptor())
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
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
