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
        public override byte[] Encrypt(byte[] plaintext)
        {
            GenerateInitializationVector();
            //System.Windows.MessageBox.Show("Length", plaintext.Length.ToString(), MessageBoxButton.OK);
            IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CBC/PKCS7Padding");

            cipher.Init(true, new ParametersWithIV(new KeyParameter(key), this.IV));
            //System.Windows.MessageBox.Show("Length", cipher.DoFinal(plaintext).Length.ToString(), MessageBoxButton.OK);

            return cipher.DoFinal(plaintext);
        }
        public override byte[] Decrypt(byte[] ciphertext)
        {
            IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CBC/PKCS7Padding");

            cipher.Init(false, new ParametersWithIV(new KeyParameter(key), this.IV));

            return cipher.DoFinal(ciphertext);
        }
    }
}
