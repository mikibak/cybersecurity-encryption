using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cybersecurity_encryption.Model
{
    internal class CBC : Encryption
    {
        public CBC() => GenerateIV();
        public override void GenerateIV()
        {
            IV = new byte[SingleBlock.BLOCK_SIZE];
            Random randGen = new Random();
            for (int i = 0; i < IV.Length; i++)
            {
                IV[i] = (byte)randGen.Next(256);
            }
        }
        public override byte[] Encrypt(byte[] key, byte[] plaintext)
        {
            byte[] encryptedByteArray = new byte[plaintext.Length];
            plaintext.CopyTo(encryptedByteArray, 0);
            byte[] tmpIV = new byte[IV.Length];
            IV.CopyTo(tmpIV, 0);

            byte[] block = new byte[SingleBlock.BLOCK_SIZE];
            long counter = 0;
            while (true)
            {
                if (counter == plaintext.Length)
                {
                    break;
                }

                block[counter % SingleBlock.BLOCK_SIZE] = plaintext[counter];
                block[counter % SingleBlock.BLOCK_SIZE] ^= tmpIV[counter%SingleBlock.BLOCK_SIZE];

                counter++;
                if (counter % SingleBlock.BLOCK_SIZE == 0)
                {
                    SingleBlock.EncryptBlock(key, block);
                    block.CopyTo(encryptedByteArray, counter - SingleBlock.BLOCK_SIZE);
                    block.CopyTo(tmpIV, 0);
                }
            }
            Padding(encryptedByteArray, true, key, tmpIV);
            return encryptedByteArray;
        }
        public override byte[] Decrypt(byte[] key, byte[] ciphertext)
        {
            byte[] encryptedByteArray = new byte[ciphertext.Length];
            ciphertext.CopyTo(encryptedByteArray, 0);
            byte[] tmpIV = new byte[IV.Length];
            byte[] nextIV = new byte[IV.Length];
            IV.CopyTo(tmpIV, 0);
            IV.CopyTo(nextIV, 0);

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
                    nextIV.CopyTo(tmpIV, 0);
                    block.CopyTo(nextIV, 0);
                    SingleBlock.EncryptBlock(key, block);

                    for(int i=0; i<SingleBlock.BLOCK_SIZE; i++)
                    {
                        block[i] ^= tmpIV[i];
                    }
                    block.CopyTo(encryptedByteArray, counter - SingleBlock.BLOCK_SIZE);
                }
            }
            Padding(encryptedByteArray, true, key, nextIV);
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
                if (isEncrypting) {
                    for(int i=0; i<block.Length; i++)
                    {
                        block[i] ^= IV[i];
                    }
                    SingleBlock.EncryptBlock(cipherKey, block);
                }
                else { 
                    SingleBlock.DecryptBlock(cipherKey, block);
                    for (int i = 0; i < block.Length; i++)
                    {
                        block[i] ^= IV[i];
                    }
                }
                block.CopyTo(byteArray, byteArray.Length - padding - 1);
            }
        }
    }
}
