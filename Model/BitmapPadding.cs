using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cybersecurity_encryption.Model
{
    internal class BitmapPadding
    {
        public static byte[] StripBitmapPadding(byte[] data)
        {
            int nOfZeros = 0;
            for (int i = data.Length - 1; i >= 0; i--)
            {
                if (data[i] == 0)
                {
                    nOfZeros++;
                }
                else break;
            }

            byte[] temp = new byte[data.Length - nOfZeros];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = data[i];
            }

            return temp;

        }

        public static byte[] AddBitmapPadding(byte[] data, int ImageWidth, int ImageHeight)
        {
            byte[] temp = new byte[(ImageHeight + 1) * ImageWidth * 4];
            for (int i = 0; i < data.Length; i++)
            {
                temp[i] = data[i];
            }
            for (int i = data.Length; i < (ImageHeight + 1) * ImageWidth * 4; i++)
            {
                temp[i] = 0;
            }
            return temp;
        }
    }
}
