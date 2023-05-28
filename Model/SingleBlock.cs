using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace cybersecurity_encryption.Model
{
    internal static class SingleBlock
    {
        public const int BLOCK_SIZE = 16;
        public static byte[] EncryptBlock(byte[] cipherKey, byte[] block)
        {
            Array.Reverse(block);
            for (int i = 0; i < block.Length; i++)
            {
                int tmpVal = (block[i] - cipherKey[i] + 256) % 256;
                block[i] = (byte)tmpVal;
            }
            return block;
        }
        public static byte[] DecryptBlock(byte[] cipherKey, byte[] block)
        {
            for (int i = 0; i < block.Length; i++)
            {
                int tmpVal = (block[i] + cipherKey[i]) % 256;
                block[i] = (byte)tmpVal;
            }
            Array.Reverse(block);
            return block;
        }
    }
}
