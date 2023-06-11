using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
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
        public abstract byte[] Encrypt(byte[] plaintext, EncryptedFile fileToEncrypt);
        public byte[] Decrypt(EncryptedFile encryptedFile) {
            byte[] ciphertext = encryptedFile.Content;
            IBufferedCipher cipher = CipherUtilities.GetCipher(encryptedFile.HashingAlgorithm + "/" + encryptedFile.EncryptionType + "/" + encryptedFile.PaddingMode);
            if(encryptedFile.IV != null)
            {
                cipher.Init(false, new ParametersWithIV(new KeyParameter(key), encryptedFile.IV));
            }
            else
            {
                cipher.Init(false, new KeyParameter(key));
            }
            return cipher.DoFinal(ciphertext);
        }
    }
}
