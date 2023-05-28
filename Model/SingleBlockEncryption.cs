using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cybersecurity_encryption.Model
{
    internal static class SingleBlockEncryption
    {
        public static byte[] EncryptBlock(byte[] key, byte[] data)
        {
            Array.Reverse(data);
            for (int i = 0; i < data.Length; i++)
            {
                int tmpVal = (data[i] - key[i] + 256) % 256;
                data[i] = (byte)tmpVal;
            }
            return data;
        }

        public static byte[] DecryptBlock(byte[] key, byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                int tmpVal = (data[i] + key[i]) % 256;
                data[i] = (byte)tmpVal;
            }
            Array.Reverse(data);
            return data;
        }
    }
}
