using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace cybersecurity_encryption.Model
{
    internal class CBC : Encryption
    {
        public CBC()
        {
        }
        public override byte[] Encrypt(byte[] plaintext, EncryptedFile fileToEncrypt)
        {
            GenerateInitializationVector();
            IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CBC/PKCS7Padding");
            fileToEncrypt.EncryptionType = "CBC";
            fileToEncrypt.HashingAlgorithm = "AES";
            fileToEncrypt.PaddingMode = "PKCS7Padding";
            fileToEncrypt.IV = this.getIV();

            cipher.Init(true, new ParametersWithIV(new KeyParameter(key), this.IV));
            return cipher.DoFinal(plaintext);
        }
    }
}
