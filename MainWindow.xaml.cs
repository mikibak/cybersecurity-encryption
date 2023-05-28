using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.Intrinsics.X86;
using System.Net;
using cybersecurity_encryption.Model;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Interop;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media.Media3D;
//using System.Windows.Forms;

namespace cybersecurity_encryption
{
    public partial class MainWindow : Window
    {
        private const int BLOCK_SIZE = 16;
        public byte[] byteArray { get; set; }
        public byte[] cipherKey = new byte[BLOCK_SIZE];

        private ECB ecb;
        private CTR ctr;
        private CBC cbc;

        public MainWindow()
        {
            InitializeComponent();

            ecb = new ECB();
            ctr = new CTR();
            cbc = new CBC();
            generateKey();
        }

        private void generateKey()
        {
            Random randGen = new Random();
            for (int i = 0; i < cipherKey.Length; i++)
            {
                cipherKey[i] = (byte)randGen.Next(256);
            }
        }
        public void SetImage(string path)
        {
            // Create source
            BitmapImage myBitmapImage = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(path);
            // To save significant application memory, set the DecodePixelWidth or
            // DecodePixelHeight of the BitmapImage value of the image source to the desired
            // height or width of the rendered image. If you don't do this, the application will
            // cache the image as though it were rendered as its normal size rather than just
            // the size that is displayed.
            // Note: In order to preserve aspect ratio, set DecodePixelWidth
            // or DecodePixelHeight but not both.
            myBitmapImage.DecodePixelWidth = (int)LoadedImage.Width;
            myBitmapImage.EndInit();

            //set image source
            LoadedImage.Source = myBitmapImage;

            Bitmap bitmap1 = new Bitmap(path);
            var arr = BitmapToArray(bitmap1.Width, bitmap1.Height, bitmap1);
            byteArray = arr;//przypisujemy
        }
        public byte[] BitmapToArray(int w, int h, Bitmap data)
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
        public Bitmap ArrayToBitmap(int w, int h, byte[] data)
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
        public BitmapImage BitmapToBitmapImage(Bitmap bitmap)
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
        private void setModifiedImage(BitmapImage myBitmapImage)
        {
            ModifiedImage.Source = myBitmapImage;
        }

        public void EncryptECB(object sender, RoutedEventArgs e)
        {
            byte [] encryptedByteArray = ecb.Encrypt(this.cipherKey, this.byteArray);
            Bitmap bitmap = ArrayToBitmap(480, 360, encryptedByteArray); //width and height hardcoded - TODO  - save bmp size to var for curr loaded bmp or save to some meta
            setModifiedImage(BitmapToBitmapImage(bitmap));
        }
        public void DecryptECB(object sender, RoutedEventArgs e)
        {
            byte[] encryptedByteArray = ecb.Decrypt(this.cipherKey, this.byteArray);
            Bitmap bitmap = ArrayToBitmap(480, 360, encryptedByteArray); //width and height hardcoded - TODO  - save bmp size to var for curr loaded bmp or save to some meta
            setModifiedImage(BitmapToBitmapImage(bitmap));
        }

        public void EncryptCBC(object sender, RoutedEventArgs e)
        {
        }
        public void DecryptCBC(object sender, RoutedEventArgs e)
        {
        }

        public void EncryptCTR(object sender, RoutedEventArgs e)
        {
        }
        public void DecryptCTR(object sender, RoutedEventArgs e)
        {
        }

        private void GetImage(object sender, RoutedEventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "bmp files (*.bmp)|*.bmp";
                DialogResult result = dlg.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    SetImage(dlg.FileName);

                }
            }
        }
    }
}
