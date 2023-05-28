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
using System.Windows.Media.Animation;
using System.DirectoryServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace cybersecurity_encryption
{
    public partial class MainWindow : Window
    {
        public byte[] byteArray { get; set; }
        public byte[] cipherKey;

        private int ImageWidth;
        private int ImageHeight;

        private Encryption ecb;
        private Encryption ctr;
        private Encryption cbc;

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
            cipherKey = new byte[SingleBlock.BLOCK_SIZE];
            Random randGen = new Random();
            for (int i = 0; i < cipherKey.Length; i++)
            {
                cipherKey[i] = (byte)randGen.Next(256);
            }
        }

        private void setModifiedImage(BitmapImage myBitmapImage)
        {
            ModifiedImage.Source = myBitmapImage;
        }
        
        public void RerollKey(object sender, RoutedEventArgs e)
        {
            generateKey();
        }

        private long Encrypt(Encryption encryption)
        {
            if(byteArray == null)
            {
                System.Windows.MessageBox.Show("Choose image to encrypt", "No image detected", MessageBoxButton.OK);
                return 0;
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            byteArray = encryption.Encrypt(this.cipherKey, this.byteArray);
            stopwatch.Stop();
            Bitmap bitmap = BitmapLoader.ArrayToBitmap(ImageWidth, ImageHeight, byteArray);
            setModifiedImage(BitmapLoader.BitmapToBitmapImage(bitmap));
            return stopwatch.ElapsedMilliseconds;
        }

        private long Decrypt(Encryption encryption)
        {
            if (byteArray == null)
            {
                System.Windows.MessageBox.Show("Choose image to decrypt", "No image detected", MessageBoxButton.OK);
                return 0;
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            byteArray = encryption.Decrypt(this.cipherKey, this.byteArray);
            stopwatch.Stop();
            Bitmap bitmap = BitmapLoader.ArrayToBitmap(ImageWidth, ImageHeight, byteArray);
            setModifiedImage(BitmapLoader.BitmapToBitmapImage(bitmap));
            return stopwatch.ElapsedMilliseconds;
        }

        public void ChangeBlockSize(object sender, RoutedEventArgs e)
        {
            SingleBlock.BLOCK_SIZE = Int32.Parse(BlockSizeVal.Text);
            cbc.GenerateIV();
            ctr.GenerateIV();
            generateKey();
        }

        public void EncryptECB(object sender, RoutedEventArgs e)
        {
            long time = Encrypt(ecb);
            ECB_Timer.Text = "ECB Time: " + time + " ms";
        }
        public void DecryptECB(object sender, RoutedEventArgs e)
        {
            long time = Decrypt(ecb);
            ECB_Timer.Text = "ECB Time: " + time + " ms";
        }

        public void EncryptCBC(object sender, RoutedEventArgs e)
        {
            long time = Encrypt(cbc);
            CBC_Timer.Text = "CBC Time: " + time + " ms";
        }
        public void DecryptCBC(object sender, RoutedEventArgs e)
        {
            long time = Decrypt(cbc);
            CBC_Timer.Text = "CBC Time: " + time + " ms";
        }

        public void EncryptCTR(object sender, RoutedEventArgs e)
        {
            long time = Encrypt(ctr);
            CTR_Timer.Text = "CTR Time: " + time + " ms";
        }
        public void DecryptCTR(object sender, RoutedEventArgs e)
        {
            long time = Decrypt(ctr);
            CTR_Timer.Text = "CTR Time: " + time + " ms";
        }

        private void GetImage(object sender, RoutedEventArgs e)
        {
            BitmapLoader bmpl = new BitmapLoader();
            if (bmpl.GetImage())
            {
                LoadedImage.Source = bmpl.SetImage(LoadedImage);
                byteArray = bmpl.GetByteArray();
                ImageWidth = bmpl.GetWidth();
                ImageHeight = bmpl.GetHeight();
            }
        }
    }
}
