using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace cybersecurity_encryption.Model
{
    internal class CTR : Encryption
    {
        public CTR()
        {
        }
        public override byte[] Encrypt(byte[] plaintext)
        {
            IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CTR/PKCS7Padding");

            cipher.Init(true, new ParametersWithIV(new KeyParameter(key), this.IV));

            return cipher.DoFinal(plaintext);
        }
        public override byte[] Decrypt(byte[] ciphertext)
        {
            IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CTR/PKCS7Padding");

            cipher.Init(false, new ParametersWithIV(new KeyParameter(key), this.IV));

            return cipher.DoFinal(ciphertext);
        }
    }
}
