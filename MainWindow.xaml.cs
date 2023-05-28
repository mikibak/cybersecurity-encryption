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

namespace cybersecurity_encryption
{
    public partial class MainWindow : Window
    {
        public byte[] byteArray { get; set; }
        public byte[] cipherKey;

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
        public void EncryptECB(object sender, RoutedEventArgs e)
        {
            byte [] encryptedByteArray = ecb.Encrypt(this.cipherKey, this.byteArray);
            byteArray = encryptedByteArray;
            Bitmap bitmap = BitmapLoader.ArrayToBitmap(480, 360, encryptedByteArray); //width and height hardcoded - TODO  - save bmp size to var for curr loaded bmp or save to some meta
            setModifiedImage(BitmapLoader.BitmapToBitmapImage(bitmap));
        }
        public void DecryptECB(object sender, RoutedEventArgs e)
        {
            byte[] decryptedByteArray = ecb.Decrypt(this.cipherKey, this.byteArray);
            byteArray=decryptedByteArray;
            Bitmap bitmap = BitmapLoader.ArrayToBitmap(480, 360, decryptedByteArray); //width and height hardcoded - TODO  - save bmp size to var for curr loaded bmp or save to some meta
            setModifiedImage(BitmapLoader.BitmapToBitmapImage(bitmap));
        }
        public void ChangeBlockSize(object sender, RoutedEventArgs e)
        {
            SingleBlock.BLOCK_SIZE = Int32.Parse(BlockSizeVal.Text);
            generateKey();
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
            BitmapLoader bmpl = new BitmapLoader();
            if (bmpl.GetImage())
            {
                LoadedImage.Source = bmpl.SetImage(LoadedImage);
                byteArray = bmpl.GetByteArray();
            }
        }
    }
}
