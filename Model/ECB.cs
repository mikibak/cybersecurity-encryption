using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cybersecurity_encryption.Model
{
    internal class ECB : Encryption
    {
        public ECB(){}

        public override byte[] Encrypt(byte[] key, byte[] message)
        {
            byte[] encryptedByteArray = new byte[message.Length];
            message.CopyTo(encryptedByteArray, 0);

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
                    SingleBlock.EncryptBlock(key, block);
                    block.CopyTo(encryptedByteArray, counter - SingleBlock.BLOCK_SIZE);
                }
            }
            return encryptedByteArray;
        }

        public override byte[] Decrypt(byte[] key, byte[] message)
        {
            byte[] encryptedByteArray = new byte[message.Length];
            message.CopyTo(encryptedByteArray, 0);

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
                    SingleBlock.DecryptBlock(key, block);
                    block.CopyTo(encryptedByteArray, counter - SingleBlock.BLOCK_SIZE);
                }
            }
            return encryptedByteArray;
        }
    }
}
