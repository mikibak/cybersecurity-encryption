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
//using System.Windows.Forms;

namespace cybersecurity_encryption
{
    public partial class MainWindow : Window
    {
        private const int BLOCK_SIZE = 16;
        public double Key { get; set; }
        public byte[] byteArray { get; set; }
        public byte[] cipherKey = new byte[BLOCK_SIZE];
        private bool isEncrypting = true; //TEMPORARY VALUE FOR TESTS - LATER CREATE TWO BUTTONS - ONE FOR ENCRYPTION AND ONE FOR DECRYPTION
        public MainWindow()
        {
            InitializeComponent();
            generateKey();
            Key = 5;
        }

        private void generateKey()
        {
            Random randGen = new Random();
            for (int i = 0; i < cipherKey.Length; i++)
            {
                cipherKey[i] = (byte)randGen.Next(256);
            }
        }

        private void KeyChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            System.Windows.Controls.TextBox t = (System.Windows.Controls.TextBox)sender;
            double i;
            if (double.TryParse(t.Text, out i) && i >= 0 && i <= 9999)
            {
                Key = i;
            }
            else
            {
                statusBox.Text = "Write correct number!";
            }
        }

        public void EncryptECB(object sender, RoutedEventArgs e)
        {
            if(byteArray != null)
            {
                Encryption ecb = new ECB(cipherKey, byteArray, BLOCK_SIZE);
                if (isEncrypting)
                {
                    byteArray = ecb.Encrypt();
                }
                else
                {
                    byteArray = ecb.Decrypt();
                }
                Bitmap bitmap = BitmapLoader.ArrayToBitmap(480, 360, byteArray); //width and height hardcoded - TODO  - save bmp size to var for curr loaded bmp or save to some meta
                setModifiedImage(BitmapLoader.BitmapToBitmapImage(bitmap));
                isEncrypting = !isEncrypting;
            } else
            {
                statusBox.Text = "Choose an image!";
            }
            
        }

        public void EncryptCBC(object sender, RoutedEventArgs e)
        {
            string path = Directory.GetCurrentDirectory();
            CBCTextBox.Text = path;
        }

        public void EncryptCTR(object sender, RoutedEventArgs e)
        {
            //This code actually calculates fibonacci sequence
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (object sender, DoWorkEventArgs args) =>
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                double previouspreviousNumber;
                double previousNumber = 0;
                double progress = 0;
                double currentNumber = 1;

                for (double i = 1; i < Key; i++)
                {
                    previouspreviousNumber = previousNumber;
                    previousNumber = currentNumber;
                    currentNumber = previouspreviousNumber + previousNumber;
                    progress = (i * 100) / Key;
                    bw.ReportProgress((int)progress);
                    Thread.Sleep(30);
                }
                args.Result = currentNumber;
            };
            bw.ProgressChanged += ((object sender, ProgressChangedEventArgs args) =>
            {
                CTRprogressBar.Value = args.ProgressPercentage;
            });
            bw.RunWorkerCompleted += ((object sender, RunWorkerCompletedEventArgs args) =>
            {
                CTRprogressBar.Value = 100;
                System.Windows.MessageBox.Show("Result: " + args.Result);
                CTRTextBox.Text = "CTR time: " + 2137;
                CTRprogressBar.Value = 0;
            });

            bw.WorkerReportsProgress = true;
            bw.RunWorkerAsync(100);
        }

        private void GetImage(object sender, RoutedEventArgs e)
        {
            BitmapLoader bmpl = new BitmapLoader();
            if(bmpl.GetImage())
            {
                LoadedImage.Source = bmpl.SetImage(LoadedImage);
                byteArray = bmpl.GetByteArray();
            }
        }

        public void setModifiedImage(BitmapImage myBitmapImage)
        {
            ModifiedImage.Source = myBitmapImage;
        }

        //this may or may not help with getting the image
        /*
                public void CompressFolder(object sender, RoutedEventArgs e)
                {
                    *//*var dlg = new System.Windows.Forms.FolderBrowserDialog() { Description = "Select directory to compress" };
                    System.Windows.Forms.DialogResult result = dlg.ShowDialog();
                    if(dlg.SelectedPath != "") {
                        DirectoryInfo root = new DirectoryInfo(dlg.SelectedPath);
                        CompressFileAndItsChildren(root);
                    }*//*
                }

                public void DecompressFolder(object sender, RoutedEventArgs e)
                {
                    *//*var dlg = new System.Windows.Forms.FolderBrowserDialog() { Description = "Select directory to decompress" };
                    System.Windows.Forms.DialogResult result = dlg.ShowDialog();
                    if (dlg.SelectedPath != null)
                    {
                        DirectoryInfo root = new DirectoryInfo(dlg.SelectedPath);
                        DecompressFileAndItsChildren(root);
                    }*//*
                }

                private static void CompressFile(string path)
                {
                    using FileStream originalFileStream = File.Open(path, FileMode.Open);
                    using FileStream compressedFileStream = File.Create(path + ".gz");
                    using var compressor = new GZipStream(compressedFileStream, CompressionMode.Compress);
                    originalFileStream.CopyTo(compressor);
                }

                private static void DecompressFile(string path)
                {
                    using FileStream compressedFileStream = File.Open(path, FileMode.Open);
                    using FileStream outputFileStream = File.Create(path.Substring(0, path.Length - 3));
                    using var decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress);
                    decompressor.CopyTo(outputFileStream);
                }*/
    }
}
