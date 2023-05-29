using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Reflection;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;

namespace cybersecurity_encryption.Model
{
    internal class ECB : Encryption
    {
        public ECB()
        {
            aes = Aes.Create();
            aes.Mode = CipherMode.ECB;
        }
    }
}
