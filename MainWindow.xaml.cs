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
        EncryptedFile file;

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
            PasswordVisible.Visibility = Visibility.Hidden;
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
            if(PasswordVisible.Visibility == Visibility.Hidden)
                cipherKey = hash.ComputeHash(Encoding.UTF8.GetBytes(PasswordInvisible.Password));
            else
                cipherKey = hash.ComputeHash(Encoding.UTF8.GetBytes(PasswordVisible.Text));
            ecb.setKey(cipherKey);
            cbc.setKey(cipherKey);
            ctr.setKey(cipherKey);
        }
        private long Encrypt(Encryption encryption)
        {
            if (byteArrayFromFile == null)
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

            file = new EncryptedFile();
            byteArrayModified = encryption.Encrypt(byteArrayFromFile);
            file.PaddingLen = byteArrayModified.Length - byteArrayFromFile.Length;
            file.Content = byteArrayModified;

            byteArrayModified = BitmapPadding.AddBitmapPadding(byteArrayModified, ImageWidth, ImageHeight);

            Bitmap bitmap = BitmapLoader.ArrayToBitmap(ImageWidth, ImageHeight + 1, byteArrayModified);
            ImageHeight = ImageHeight + 1;
            changedImage = bitmap;
            setModifiedImage(BitmapLoader.BitmapToBitmapImage(bitmap));

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
                byteArrayModified = encryption.Decrypt(file.Content);
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

        public void HidePassword(object sender, RoutedEventArgs e)
        {
            PasswordInvisible.Password = PasswordVisible.Text;
            PasswordInvisible.Visibility = Visibility.Visible;
            PasswordVisible.Visibility = Visibility.Hidden;
        }
        public void ShowPassword(object sender, RoutedEventArgs e)
        {
            PasswordVisible.Text = PasswordInvisible.Password;
            PasswordVisible.Visibility = Visibility.Visible;
            PasswordInvisible.Visibility = Visibility.Hidden;
        }

        public void EncryptECB(object sender, RoutedEventArgs e)
        {
            long time = Encrypt(ecb);
            file.EncryptionType = "ECB";
            ECB_Timer.Text = "ECB Time: " + time + " ms";
        }
        public void EncryptCBC(object sender, RoutedEventArgs e)
        {
            long time = Encrypt(cbc);
            file.EncryptionType = "CBC";
            file.IV = cbc.getIV();
            CBC_Timer.Text = "CBC Time: " + time + " ms";
        }
        public void EncryptCTR(object sender, RoutedEventArgs e)
        {
            long time = Encrypt(ctr);
            file.EncryptionType = "CTR";
            file.IV = ctr.getIV();
            CTR_Timer.Text = "CTR Time: " + time + " ms";
        }
        public void Decrypt(object sender, RoutedEventArgs e)
        {
            long time;
            switch (file.EncryptionType)
            {
                case "ECB":
                    time = Decrypt(ecb);
                    ECB_Timer.Text = "ECB Time: " + time + " ms";
                    break;
                case "CBC":
                    time = Decrypt(cbc);
                    CBC_Timer.Text = "CBC Time: " + time + " ms";
                    break;
                case "CTR":
                    time = Decrypt(ctr);
                    CTR_Timer.Text = "CTR Time: " + time + " ms";
                    break;
            }
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
            //FileHandler fileHanlder = new FileHandler();
            //fileHanlder.SaveFile(changedImage, SavedFileName.Text);
            file.SaveToFile(SavedFileName.Text);
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
