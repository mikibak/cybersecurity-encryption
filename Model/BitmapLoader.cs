using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Imaging;

namespace cybersecurity_encryption.Model
{
    public class BitmapLoader
    {
        private string FileName = "";
        private bool FileEncrypted = false;
        Bitmap? bitmap;

        public int GetHeight()
        {
            if (bitmap == null) return -1;
            return bitmap.Height;
        }

        public int GetWidth() {
            if (bitmap == null) return -1;
            return bitmap.Width;
        }

        public bool GetImage()
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "files to encrypt (*.bmp)|*.bmp";
                DialogResult result = dlg.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    FileName = dlg.FileName;
                    FileEncrypted = FileName.Contains(".xml");
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool GetEncryptedFile()
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "files to decrypt (*.xml)|*.xml";
                DialogResult result = dlg.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    FileName = dlg.FileName;
                    FileEncrypted = FileName.Contains(".xml");
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public BitmapImage SetImage(System.Windows.Controls.Image loadedImage)
        {
            // Create source
            BitmapImage myBitmapImage = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(FileName);
            myBitmapImage.DecodePixelWidth = (int)loadedImage.Width;
            myBitmapImage.EndInit();

            //set image source
            return myBitmapImage;
        }

        public byte[] GetByteArray()
        {
            bitmap = new Bitmap(FileName);
            var arr = BitmapToArray(bitmap.Width, bitmap.Height, bitmap);
            return arr;
        }

        public static byte[] BitmapToArray(int w, int h, Bitmap data)
        {
            byte[] dat = new byte[w * h * 4];
            int iterator = 0;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    //get pixel value
                    System.Drawing.Color p = data.GetPixel(x, y);

                    //extract ARGB value from p
                    dat[iterator] = p.A;
                    dat[iterator + 1] = p.R;
                    dat[iterator + 2] = p.G;
                    dat[iterator + 3] = p.B;
                    iterator += 4;
                }
            }
            return dat;
        }
        public static Bitmap ArrayToBitmap(int w, int h, byte[] data)
        {
            Bitmap pic = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int arrayIndex = 0;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    System.Drawing.Color c = System.Drawing.Color.FromArgb(
                       data[arrayIndex],
                       data[arrayIndex + 1],
                       data[arrayIndex + 2],
                       data[arrayIndex + 3]
                    );
                    arrayIndex = arrayIndex + 4;
                    pic.SetPixel(x, y, c);
                }
            }
            return pic;
        }

        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();

            // Save the Bitmap to a MemoryStream using PNG format
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            ms.Seek(0, SeekOrigin.Begin);

            // Use the MemoryStream as the source for the BitmapImage
            bi.StreamSource = ms;

            bi.EndInit();
            return bi;
        }

    }
}