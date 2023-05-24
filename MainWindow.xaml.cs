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
//using System.Windows.Forms;

namespace cybersecurity_encryption
{
    public partial class MainWindow : Window
    {
        public double N { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            N = 5;
        }

        private void KeyChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            TextBox t = (TextBox)sender;
            double i;
            if(double.TryParse(t.Text, out i) && i >= 0 && i <= 9999)
            {
                N = i;
            }
            else
            {
                statusBox.Text = "Write correct number!";
            }
        }

        public void EncryptECB(object sender, RoutedEventArgs e)
        {

        }

        public void EncryptCBC(object sender, RoutedEventArgs e)
        {

        }

        public void EncryptCTR(object sender, RoutedEventArgs e)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (object sender, DoWorkEventArgs args) =>
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                double previouspreviousNumber;
                double previousNumber = 0;
                double progress = 0;
                double currentNumber = 1;

                for (double i = 1; i < N; i++) {
                    previouspreviousNumber = previousNumber;
                    previousNumber = currentNumber;
                    currentNumber = previouspreviousNumber + previousNumber;
                    progress = (i* 100) / N;
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
                MessageBox.Show("Result: " + args.Result);
                CTRTextBox.Text = "CTR time: " + 2137;
                CTRprogressBar.Value = 0;
            });

            bw.WorkerReportsProgress = true;
            bw.RunWorkerAsync(100);
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
