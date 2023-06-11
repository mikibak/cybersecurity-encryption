using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace cybersecurity_encryption.Model
{
    internal class ECB : Encryption
    {
        public ECB()
        {
        }
        public override byte[] Encrypt(byte[] plaintext, EncryptedFile fileToEncrypt)
        {
            IBufferedCipher cipher = CipherUtilities.GetCipher("AES/ECB/PKCS7Padding");
            fileToEncrypt.EncryptionType = "ECB";
            fileToEncrypt.HashingAlgorithm = "AES";
            fileToEncrypt.PaddingMode = "PKCS7Padding";

            cipher.Init(true, new KeyParameter(key));
            return cipher.DoFinal(plaintext);
        }
    }
}
