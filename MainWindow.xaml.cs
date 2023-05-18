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
        public double K { get; set; }

        string[] hostNames = { "www.microsoft.com", "www.apple.com",
            "www.google.com", "www.ibm.com", "cisco.netacad.net",
            "www.oracle.com", "www.nokia.com", "www.hp.com", "www.dell.com",
            "www.samsung.com", "www.toshiba.com", "www.siemens.com",
            "www.amazon.com", "www.sony.com", "www.canon.com", 
            "www.alcatel-lucent.com", "www.acer.com", "www.motorola.com" };

        public MainWindow()
        {
            InitializeComponent();
            N = 5;
            K = 4;
            //NewtonSymbol newtonSymbol = new NewtonSymbol(5, 4, calculateNewtonSymbolTasksTextBox);
        }

        private void nChangedEventHandler(object sender, TextChangedEventArgs args)
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

        private void kChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            TextBox t = (TextBox)sender;
            double i;
            if (double.TryParse(t.Text, out i) && i >= 0 && i <= 9999)
            {
                K = i;
            }
            else
            {
                statusBox.Text = "Write correct number!";
            }
        }

        public void CalculateNewtonSymbolTasks(object sender, RoutedEventArgs e)
        {
            (double, double) tuple = (N, K);
            Task<double> taskUpper = new Task<double>((object obj) =>
            {
                double result = 1;
                for (double i = 1; i <= N; i++)
                {
                    result *= i;
                }
                return result;
            }, N);
            taskUpper.Start();
            taskUpper.Wait();

            Task<double> taskLower = new Task<double>((object obj) =>
            {
                double result = 1;
                for (double i = 1; i <= K; i++)
                {
                    result *= i;
                }
                return result;
            }, K);
            taskLower.Start();
            taskLower.Wait();

            Task<double> taskLower2 = new Task<double>((object obj) =>
            {
                double result = 1;
                for (double i = 1; i <= (N - K); i++)
                {
                    result *= i;
                }
                return result;
            }, tuple);
            taskLower2.Start();
            taskLower2.Wait();

            double result = taskUpper.Result / (taskLower.Result * taskLower2.Result);
            calculateNewtonSymbolTasksTextBox.Text = result.ToString();
        }

        public void CalculateNewtonSymbolDelegates(object sender, RoutedEventArgs e)
        {
            var result = CalculateNewtonSymbolDelegatesHelper();
            calculateNewtonSymbolDelegatesTextBox.Text = result.ToString();
        }

        public double CalculateNewtonSymbolDelegatesHelper()
        {
            Func<double> upperDelegate = CalculateUpper;
            Func<double> lowerDelegate = CalculateLower;
            Func<double> lower2Delegate = CalculateLower2;

            var upper = upperDelegate.BeginInvoke(null, null);
            var lower = lowerDelegate.BeginInvoke(null, null);
            var lower2 = lower2Delegate.BeginInvoke(null, null);
            return upperDelegate.EndInvoke(upper) / (lowerDelegate.EndInvoke(lower) * lower2Delegate.EndInvoke(lower2));
        }

        private static List<Tuple<string, string>> GetAdresses(string[] hostNames)
        {
            List<Tuple<string, string>> result = new List<Tuple<string, string>>();
            hostNames.AsParallel()
                .ForAll(hostName =>
                {
                    lock (result)
                    {
                        result.Add(Tuple.Create(hostName, Dns.GetHostAddresses(hostName).First().ToString()));
                    }
                });

            return result;
        }

        public void ResolveDNS(object sender, RoutedEventArgs args)
        {
            var domainList = GetAdresses(hostNames);
            resolveDNSTextBox.Text = "";
            foreach (var domain in domainList)
            {
                resolveDNSTextBox.Text += $"{domain.Item1} => {domain.Item2}\n";
            }
        }

        public async void CalculateNewtonSymbolAsyncAwait(object sender, RoutedEventArgs e)
        {
            var upper = await Task.Run(() => CalculateUpper());
            var lower = await Task.Run(() => CalculateLower());
            var lower2 = await Task.Run(() => CalculateLower2());
            double result = upper / (lower * lower2);
            calculateNewtonSymbolAsyncAwaitTextBox.Text = result.ToString();
        }

        private double CalculateUpper()
        {
            double result = 1;
            for (double i = 1; i <= N; i++)
            {
                result *= i;
            }
            return result;
        }

        private double CalculateLower()
        {
            double result = 1;
            for (double i = 1; i <= K; i++)
            {
                result *= i;
            }
            return result;
        }

        private double CalculateLower2()
        {
            double result = 1;
            for (double i = 1; i <= (N - K); i++)
            {
                result *= i;
            }
            return result;
        }

        public void CalculateFibonacci(object sender, RoutedEventArgs e)
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
                progressBar.Value = args.ProgressPercentage;
            });
            bw.RunWorkerCompleted += ((object sender, RunWorkerCompletedEventArgs args) =>
            {
                progressBar.Value = 100;
                MessageBox.Show("Result: " + args.Result);
                progressBar.Value = 0;
            });

            bw.WorkerReportsProgress = true;
            bw.RunWorkerAsync(100);

            //double result = 0;
            //calculateFibonacciTextBox.Text = result.ToString();
        }

        public void CompressFolder(object sender, RoutedEventArgs e)
        {
            /*var dlg = new System.Windows.Forms.FolderBrowserDialog() { Description = "Select directory to compress" };
            System.Windows.Forms.DialogResult result = dlg.ShowDialog();
            if(dlg.SelectedPath != "") {
                DirectoryInfo root = new DirectoryInfo(dlg.SelectedPath);
                CompressFileAndItsChildren(root);
            }*/
        }

        public void DecompressFolder(object sender, RoutedEventArgs e)
        {
            /*var dlg = new System.Windows.Forms.FolderBrowserDialog() { Description = "Select directory to decompress" };
            System.Windows.Forms.DialogResult result = dlg.ShowDialog();
            if (dlg.SelectedPath != null)
            {
                DirectoryInfo root = new DirectoryInfo(dlg.SelectedPath);
                DecompressFileAndItsChildren(root);
            }*/
        }

        private static void CompressFileAndItsChildren(DirectoryInfo root)
        {
            
            DirectoryInfo[] Dirs = root.GetDirectories();
            FileInfo[] Files = root.GetFiles("*");
            Parallel.For(0, Files.Length,
                   index => {
                       CompressFile(Files[index].FullName);
                       File.Delete(Files[index].FullName);
                   });
            foreach (var dir in Dirs)
            {
                CompressFileAndItsChildren(dir);
            }
        }

        private static void DecompressFileAndItsChildren(DirectoryInfo root)
        {

            DirectoryInfo[] Dirs = root.GetDirectories();
            FileInfo[] Files = root.GetFiles("*");
            Parallel.For(0, Files.Length,
                   index => {
                       DecompressFile(Files[index].FullName);
                       File.Delete(Files[index].FullName);
                   });
            foreach (var dir in Dirs)
            {
                DecompressFileAndItsChildren(dir);
            }
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
        }
    }
}
