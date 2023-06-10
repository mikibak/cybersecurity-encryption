using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace cybersecurity_encryption.Model
{
    internal class CTR : Encryption
    {
        public CTR()
        {
        }
        public override byte[] Encrypt(byte[] plaintext)
        {
            GenerateInitializationVector();
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
