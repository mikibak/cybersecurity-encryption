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
            aes = Aes.Create();
            aes.Mode = CipherMode.ECB;
        }
        byte[] EncryptBlock(byte[] block, byte[] key, byte[] iv)
        {
            ICryptoTransform encryptor = aes.CreateEncryptor();
            byte[] encryptedBlock = new byte[block.Length];
            encryptor.TransformBlock(block, 0, block.Length, encryptedBlock, 0);
            return encryptedBlock;
        }
        byte[] GenerateCounter(byte[] iv, int blockIndex)
        {
            byte[] counter = new byte[iv.Length];
            Buffer.BlockCopy(iv, 0, counter, 0, iv.Length);

            byte[] indexBytes = BitConverter.GetBytes(blockIndex);
            Array.Reverse(indexBytes); // Revert the bytes to maintain big-endian order

            for (int i = 0; i < indexBytes.Length; i++)
            {
                counter[counter.Length - 1 - i] ^= indexBytes[i];
            }

            return counter;
        }
        public override byte[] Encrypt(byte[] plaintext)
        {
            byte[] encryptedData = new byte[plaintext.Length];
            int BLOCK_SIZE = aes.IV.Length;
            for (int i = 0; i < plaintext.Length; i += BLOCK_SIZE)
            {
                byte[] block = new byte[BLOCK_SIZE];
                Buffer.BlockCopy(plaintext, i, block, 0, BLOCK_SIZE);

                byte[] counter = GenerateCounter(aes.IV, i / BLOCK_SIZE);
                byte[] encryptedBlock = EncryptBlock(counter, aes.Key, aes.IV);

                for (int j = 0; j < BLOCK_SIZE; j++)
                {
                    encryptedData[i + j] = (byte)(block[j] ^ encryptedBlock[j]);
                }
            }

            return encryptedData;
        }
        public override byte[] Decrypt(byte[] ciphertext)
        {
            return Encrypt(ciphertext);
        }
    }
}
