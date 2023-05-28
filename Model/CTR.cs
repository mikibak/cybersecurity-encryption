using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cybersecurity_encryption.Model
{
    internal class CTR : Encryption
    {
        public CTR() => GenerateIV();
        public byte[] IV;
        public override void GenerateIV()
        {
            IV = new byte[SingleBlock.BLOCK_SIZE];
            Random randGen = new Random();
            for (int i = 0; i < IV.Length; i++)
            {
                IV[i] = (byte)randGen.Next(256);
            }
        }
        private void incCounterVec(byte[] iv)
        {
            for(int i=0; i < iv.Length; i++)
            {
                if (iv[i] < 255)
                {
                    iv[i] += 1;
                    break;
                }
            }
        }
        public override byte[] Encrypt(byte[] key, byte[] plaintext)
        {
            byte[] encryptedByteArray = new byte[plaintext.Length];
            plaintext.CopyTo(encryptedByteArray, 0);
            byte[] tmpIV = new byte[IV.Length];
            IV.CopyTo(tmpIV, 0);
            
            byte[] block = new byte[SingleBlock.BLOCK_SIZE];
            long iterator = 0;
            long counter = 0;
            while (true)
            {
                if (iterator == plaintext.Length)
                {
                    break;
                }
                block[iterator % SingleBlock.BLOCK_SIZE] = plaintext[iterator];
                iterator++;
                if (iterator % SingleBlock.BLOCK_SIZE == 0)
                {
                    incCounterVec(tmpIV);
                    SingleBlock.EncryptBlock(key, tmpIV);
                    for(int i=0; i<SingleBlock.BLOCK_SIZE; i++)
                    {
                        block[i] ^= tmpIV[i];
                    }
                    block.CopyTo(encryptedByteArray, iterator - SingleBlock.BLOCK_SIZE);
                    counter++;
                }
            }
            incCounterVec(tmpIV);
            Padding(encryptedByteArray, true, key, tmpIV);
            return encryptedByteArray;
        }
        public override byte[] Decrypt(byte[] key, byte[] ciphertext)
        {
            return Encrypt(key, ciphertext);
        }
        public override void Padding(byte[] byteArray, bool isEncrypting, byte[] cipherKey, byte[]? IV) {
            int padding = byteArray.Length % SingleBlock.BLOCK_SIZE;
            if (padding != 0)
            {
                byte[] block = new byte[padding];
                for (int i = (int)(byteArray.Length - padding); i < byteArray.Length; i++)
                {
                    block[i % padding] = (byte)byteArray[i];
                }
                SingleBlock.EncryptBlock(cipherKey, IV);
                for (int i = 0; i < block.Length; i++)
                {
                    block[i] ^= IV[i];
                }
                block.CopyTo(byteArray, byteArray.Length - padding - 1);
            }
        }
    }
}
