using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace cybersecurity_encryption.Model
{
    public class FileHandler
    {
        public Tuple<byte[], Encryption, Encryption> ReadKeyFromFile(Encryption cbc, Encryption ctr)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Key";
                DialogResult result = dlg.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    var lineCount = File.ReadLines(dlg.FileName).Count();
                    using (StreamReader sr = File.OpenText(dlg.FileName))
                    {
                        byte[] cipherKey_temp = new byte[lineCount];
                        String s = "";
                        int iter = 0;
                        while ((s = sr.ReadLine()) != null)
                        {
                            if (iter < lineCount / 3)
                                cipherKey_temp[iter] = byte.Parse(s);
                           /* else if (iter >= lineCount / 3 && iter < (lineCount / 3 * 2)) 
                            cbc.IV[iter - lineCount / 3] = byte.Parse(s);
                            else
                                ctr.IV[iter - lineCount / 3 * 2] = byte.Parse(s);*/
                                iter++;
                        }
                        return Tuple.Create(cipherKey_temp,cbc,ctr);
                    }
                }
                return null;
            }
        }
        public void SaveFile(Bitmap changedImage,string name)
        {
            if (changedImage != null && !String.Equals(name, ""))
            {
                //string path = Directory.GetCurrentDirectory();
                changedImage.Save(("../../../Resources/" + name + ".bmp"));
                /*string path = @"../../../Resources/" + name + ".txt";
                using (StreamWriter sw = File.CreateText(path))
                {
                    foreach (byte b in BitmapLoader.BitmapToArray(changedImage.Width, changedImage.Height, changedImage))
                        sw.WriteLine(b);
                }*/
            }
        }
    }
}
