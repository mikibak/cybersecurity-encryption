using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cybersecurity_encryption.Model
{
    internal class ECB : Encryption
    {
        public ECB(byte[] key, byte[] message, int blockSize) : base(key, message, blockSize) {}

        public override byte[] Encrypt()
        {
            byte[] encryptedByteArray = new byte[Message.Length];
            Message.CopyTo(encryptedByteArray, 0);

            byte[] block = new byte[BlockSize];
            long counter = 0;
            while (true)
            {
                if (counter == Message.Length)
                {
                    break;
                }
                block[counter % BlockSize] = Message[counter];
                counter++;
                if (counter % BlockSize == 0)
                {
                    SingleBlockEncryption.EncryptBlock(Key, block);
                    block.CopyTo(encryptedByteArray, counter - BlockSize);
                }
            }
            //encryptedByteArray.CopyTo(Message, 0);
            return encryptedByteArray;
           
        }
        public override byte[] Decrypt()
        {
            byte[] decryptedByteArray = new byte[Message.Length];
            Message.CopyTo(decryptedByteArray, 0);

            byte[] block = new byte[BlockSize];
            long counter = 0;
            while (true)
            {
                if (counter == Message.Length)
                {
                    break;
                }
                block[counter % BlockSize] = Message[counter];
                counter++;
                if (counter % BlockSize == 0)
                {
                    SingleBlockEncryption.DecryptBlock(Key, block);
                    block.CopyTo(decryptedByteArray, counter - BlockSize);
                }
            }
            //encryptedByteArray.CopyTo(Message, 0);
            return decryptedByteArray;
        }
    }
}
