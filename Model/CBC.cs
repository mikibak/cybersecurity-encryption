using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cybersecurity_encryption.Model
{
    internal class CBC : Encryption
    {
        public CBC(){
            generateIV();
        }
        public byte[] IV;
        private void generateIV()
        {
            IV = new byte[SingleBlock.BLOCK_SIZE];
            Random randGen = new Random();
            for (int i = 0; i < IV.Length; i++)
            {
                IV[i] = (byte)randGen.Next(256);
            }
        }
        public override byte[] Encrypt(byte[] key, byte[] message)
        {
            byte[] encryptedByteArray = new byte[message.Length];
            message.CopyTo(encryptedByteArray, 0);
            byte[] tmpIV = new byte[IV.Length];
            IV.CopyTo(tmpIV, 0);

            byte[] block = new byte[SingleBlock.BLOCK_SIZE];
            long counter = 0;
            while (true)
            {
                if (counter == message.Length)
                {
                    break;
                }

                block[counter % SingleBlock.BLOCK_SIZE] = message[counter];
                block[counter % SingleBlock.BLOCK_SIZE] ^= tmpIV[counter%SingleBlock.BLOCK_SIZE];

                counter++;
                if (counter % SingleBlock.BLOCK_SIZE == 0)
                {
                    SingleBlock.EncryptBlock(key, block);
                    block.CopyTo(encryptedByteArray, counter - SingleBlock.BLOCK_SIZE);
                    block.CopyTo(tmpIV, 0);
                }
            }
            //SingleBlock.Padding(encryptedByteArray, true, key); TODO rework padding
            return encryptedByteArray;
        }
        public override byte[] Decrypt(byte[] key, byte[] message)
        {
            byte[] encryptedByteArray = new byte[message.Length];
            message.CopyTo(encryptedByteArray, 0);
            byte[] tmpIV = new byte[IV.Length];
            byte[] nextIV = new byte[IV.Length];
            IV.CopyTo(tmpIV, 0);
            IV.CopyTo(nextIV, 0);

            byte[] block = new byte[SingleBlock.BLOCK_SIZE];
            long counter = 0;
            while (true)
            {
                if (counter == message.Length)
                {
                    break;
                }
                block[counter % SingleBlock.BLOCK_SIZE] = message[counter];
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
            //SingleBlock.Padding(encryptedByteArray, true, key); TODO rework padding
            return encryptedByteArray;
        }
    }
}
