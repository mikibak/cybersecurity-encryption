using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Reflection;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace cybersecurity_encryption.Model
{
    internal class ECB : Encryption
    {
        public ECB()
        {
        }
        public override byte[] Encrypt(byte[] plaintext)
        {
            IBufferedCipher cipher = CipherUtilities.GetCipher("AES/ECB/PKCS7Padding");

            cipher.Init(true, new KeyParameter(key));

            return cipher.DoFinal(plaintext);
        }
        public override byte[] Decrypt(byte[] ciphertext)
        {
            IBufferedCipher cipher = CipherUtilities.GetCipher("AES/ECB/PKCS7Padding");

            cipher.Init(false, new KeyParameter(key));

            return cipher.DoFinal(ciphertext);
        }
    }
}
