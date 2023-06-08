using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using cybersecurity_encryption.Model;
using System.Drawing;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;

namespace cybersecurity_encryption
{
    public partial class MainWindow : Window
    {
        public byte[] byteArrayFromFile { get; set; }
        public byte[] byteArrayModified { get; set; }
        public byte[] debugArray { get; set; }
        public byte[] cipherKey;

        private int ImageWidth;
        private int ImageHeight;

        private Encryption ecb;
        private Encryption ctr;
        private Encryption cbc;
        private HashAlgorithm hash;

        Bitmap changedImage;
        public MainWindow()
        {
            InitializeComponent();

            ecb = new ECB();
            ctr = new CTR();
            cbc = new CBC();
        }

        private void setModifiedImage(BitmapImage myBitmapImage)
        {
            ModifiedImage.Source = myBitmapImage;
        }
        public void GenerateKey(object sender, RoutedEventArgs e)
        {
            hash = MD5.Create();
            cipherKey = hash.ComputeHash(Encoding.UTF8.GetBytes(PasswordVal.Text));
            ecb.setKey(cipherKey);
            cbc.setKey(cipherKey);
            ctr.setKey(cipherKey);
        }
        private long Encrypt(Encryption encryption)
        {
            if(byteArrayFromFile == null)
            {
                System.Windows.MessageBox.Show("Choose image to encrypt", "No image detected", MessageBoxButton.OK);
                return 0;
            }
            else if (hash == null)
            {
                System.Windows.MessageBox.Show("Enter password for key generation", "No key generated", MessageBoxButton.OK);
                return 0;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            byteArrayModified = encryption.Encrypt(byteArrayFromFile);

            byteArrayModified = BitmapPadding.AddBitmapPadding(byteArrayModified, ImageWidth, ImageHeight);

            Bitmap bitmap = BitmapLoader.ArrayToBitmap(ImageWidth, ImageHeight + 1, byteArrayModified);
            ImageHeight = ImageHeight + 1;
            changedImage = bitmap;
            setModifiedImage(BitmapLoader.BitmapToBitmapImage(bitmap));

            //NEW CODE - PURELY FOR TEST PURPOSES
            EncryptedFile file = new EncryptedFile(cipherKey, byteArrayModified, 3);
            file.SaveToFile("example");
            file = null;
            file = EncryptedFile.ReadFromFile("example");


            return stopwatch.ElapsedMilliseconds;
        }

        private long Decrypt(Encryption encryption)
        {
            try
            {
                if (byteArrayFromFile == null)
                {
                    System.Windows.MessageBox.Show("Choose image to decrypt", "No image detected", MessageBoxButton.OK);
                    return 0;
                }
                else if (hash == null)
                {
                    System.Windows.MessageBox.Show("Enter password for key generation", "No key generated", MessageBoxButton.OK);
                    return 0;
                }

                //remove padding
                byteArrayFromFile = BitmapPadding.StripBitmapPadding(byteArrayFromFile);
                
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                byteArrayModified = encryption.Decrypt(byteArrayFromFile);
                stopwatch.Stop();
                Bitmap bitmap = BitmapLoader.ArrayToBitmap(ImageWidth, ImageHeight - 1, byteArrayModified);
                ImageHeight = ImageHeight - 1;
                changedImage = bitmap;
                setModifiedImage(BitmapLoader.BitmapToBitmapImage(bitmap));
                return stopwatch.ElapsedMilliseconds;
            }
            catch (InvalidCipherTextException ex)
            {
                System.Windows.MessageBox.Show("Decryption Error" + ex, "Invalid Ciphertext", MessageBoxButton.OK);
                return -1;
            }
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
                byteArrayFromFile = bmpl.GetByteArray();
                ImageWidth = bmpl.GetWidth();
                ImageHeight = bmpl.GetHeight();
            }
        }

        private void SaveChangedImage_Click(object sender, RoutedEventArgs e)
        {
            FileHandler fileHanlder = new FileHandler();
            fileHanlder.SaveFile(changedImage, SavedFileName.Text);
        }

        private void ReadyKeyFile_Click(object sender, RoutedEventArgs e)
        {
            FileHandler fileHanlder = new FileHandler();
            var result = fileHanlder.ReadKeyFromFile(cbc, ctr);
            if (result != null)
            {
                cipherKey = result.Item1;
                cbc = result.Item2;
                ctr=result.Item3;
            }
        }
    }
}
