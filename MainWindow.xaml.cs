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
        public double Key { get; set; }
        public byte[] byteArray { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            //SetImage("C:\\Users\\mikolaj\\cybersecurity-encryption\\Resources\\default.bmp");
            Key = 5;
        }

        private void KeyChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            System.Windows.Controls.TextBox t = (System.Windows.Controls.TextBox)sender;
            double i;
            if(double.TryParse(t.Text, out i) && i >= 0 && i <= 9999)
            {
                Key = i;
            }
            else
            {
                statusBox.Text = "Write correct number!";
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
            myBitmapImage.DecodePixelWidth = 200;
            myBitmapImage.EndInit();
            //set image source
            //Image.Source = myBitmapImage;

            Bitmap bitmap1 = new Bitmap(path);
            var arr = BitmapToArray(bitmap1.Width,bitmap1.Height, bitmap1);
            byteArray = arr;//przypisujemy 
            for (int i=0;i<arr.Length;i++)//for fun 
            {
                arr[i] = (byte)(255 -arr[i]);
            }
            Bitmap bmp = ArrayToBitmap(bitmap1.Width, bitmap1.Height, arr);
            Image.Source = BitmapToBitmapImage(bmp);

        }
        public byte[] BitmapToArray(int w, int h, Bitmap data)
        {
            byte[] dat = new byte[w * h * 4];
            int iteratorr = 0;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    //get pixel value
                    System.Drawing.Color p = data.GetPixel(x, y);

                    //extract ARGB value from p
                    dat[iteratorr] = p.A;
                    dat[iteratorr + 1] = p.R;
                    dat[iteratorr + 2] = p.G;
                    dat[iteratorr + 3] = p.B;
                    iteratorr = iteratorr + 4;
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
                       data[arrayIndex +1],
                       data[arrayIndex + 2],
                       data[arrayIndex + 3]
                    );
                    arrayIndex = arrayIndex + 4;
                    pic.SetPixel(x, y, c);
                }
            }
            return pic;
        }

        public  BitmapImage BitmapToBitmapImage( Bitmap bitmap)
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
        

        public void EncryptECB(object sender, RoutedEventArgs e)
        {

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

                for (double i = 1; i < Key; i++) {
                    previouspreviousNumber = previousNumber;
                    previousNumber = currentNumber;
                    currentNumber = previouspreviousNumber + previousNumber;
                    progress = (i* 100) / Key;
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
