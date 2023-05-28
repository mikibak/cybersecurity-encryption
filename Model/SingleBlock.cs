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
    }
}
