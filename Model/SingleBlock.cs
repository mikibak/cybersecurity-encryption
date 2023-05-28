using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;

namespace cybersecurity_encryption.Model
{
    internal static class SingleBlock
    {
        public static int BLOCK_SIZE { get; set; } = 16;
    public static byte[] EncryptBlock(byte[] cipherKey, byte[] block)
        {
            for (int i = 0; i < block.Length; i++)
            {
                block[i] = (byte)(block[i] ^ cipherKey[i]);
            }
            return block;
        }
        public static byte[] DecryptBlock(byte[] cipherKey, byte[] block)
        {
            for (int i = 0; i < block.Length; i++)
            {
                block[i] = (byte)(block[i] ^ cipherKey[i]);
            }
            return block;
        }
        public static void Padding(byte[] byteArray, bool isEncrypting, byte[] cipherKey)
        {
            int padding = byteArray.Length % BLOCK_SIZE;
            byte[] block = new byte[padding];
            for (int i = (int)(byteArray.Length - padding); i < byteArray.Length; i++)
            {
                block[i % padding] = (byte)byteArray[i];
            }
            if (padding != 0)
            {
                if(isEncrypting) { SingleBlock.EncryptBlock(cipherKey, block); }
                else { SingleBlock.DecryptBlock(cipherKey, block); }

                block.CopyTo(byteArray, byteArray.Length - padding - 1);
            }
        }
    }
}
