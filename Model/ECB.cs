using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cybersecurity_encryption.Model
{
    internal class ECB : Encryption
    {
        public ECB(){}

        public override byte[] Encrypt(byte[] key, byte[] plaintext)
        {
            byte[] encryptedByteArray = new byte[plaintext.Length];
            plaintext.CopyTo(encryptedByteArray, 0);

            byte[] block = new byte[SingleBlock.BLOCK_SIZE];
            long counter = 0;
            while (true)
            {
                if (counter == plaintext.Length)
                {
                    break;
                }
                block[counter % SingleBlock.BLOCK_SIZE] = plaintext[counter];
                counter++;
                if (counter % SingleBlock.BLOCK_SIZE == 0)
                {
                    SingleBlock.EncryptBlock(key, block);
                    block.CopyTo(encryptedByteArray, counter - SingleBlock.BLOCK_SIZE);
                }
            }
            Padding(encryptedByteArray, true, key, null);
            return encryptedByteArray;
        }

        public override byte[] Decrypt(byte[] key, byte[] ciphertext)
        {
            byte[] encryptedByteArray = new byte[ciphertext.Length];
            ciphertext.CopyTo(encryptedByteArray, 0);

            byte[] block = new byte[SingleBlock.BLOCK_SIZE];
            long counter = 0;
            while (true)
            {
                if (counter == ciphertext.Length)
                {
                    break;
                }
                block[counter % SingleBlock.BLOCK_SIZE] = ciphertext[counter];
                counter++;
                if (counter % SingleBlock.BLOCK_SIZE == 0)
                {
                    SingleBlock.DecryptBlock(key, block);
                    block.CopyTo(encryptedByteArray, counter - SingleBlock.BLOCK_SIZE);
                }
            }
            Padding(encryptedByteArray, false, key, null);
            return encryptedByteArray;
        }
        public override void Padding(byte[] byteArray, bool isEncrypting, byte[] cipherKey, byte[]? IV)
        {
            int padding = byteArray.Length % SingleBlock.BLOCK_SIZE;
            if (padding != 0)
            {
                byte[] block = new byte[padding];
                for (int i = (int)(byteArray.Length - padding); i < byteArray.Length; i++)
                {
                    block[i % padding] = (byte)byteArray[i];
                }
                if (isEncrypting) { SingleBlock.EncryptBlock(cipherKey, block); }
                else { SingleBlock.DecryptBlock(cipherKey, block); }

                block.CopyTo(byteArray, byteArray.Length - padding - 1);
            }
        }
    }
}
