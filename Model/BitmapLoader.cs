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
        public bool GetImage()
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "bmp files (*.bmp)|*.bmp";
                DialogResult result = dlg.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    FileName = dlg.FileName;
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
            Bitmap bitmap1 = new Bitmap(FileName);
            var arr = BitmapToArray(bitmap1.Width, bitmap1.Height, bitmap1);
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
            Bitmap pic = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
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
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }

    }
}
