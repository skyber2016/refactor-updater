using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Navigation;
using Update.Classes.Functions;
using Update.ViewModels;

namespace Update.Forms
{
    public partial class MainWindow : Window
    {
        private MainViewModel ViewModel = new MainViewModel();

        public static string launcher_hostname = "https://update.zeroonlinevn.com/";

        public static string launcher_ininame = "resources.xml";

        public static string launcher_archive = "m3i}J}jyAdm24ltg";

        public static string launcher_constructor = "Whaa%AcOT-{MJJlSadOzQSY%0Bvq/Y4T";

        public static string client_cabalmain = "Zero.exe";

        public static string client_cabalmain2 = "ZeroOnline.exe";

        public static string client_hostname = launcher_hostname;

        public static string dir_current = Directory.GetCurrentDirectory() + "\\";

        public static string dir_temp = Path.GetTempPath() + "\\";

        public bool is_working;

        private bool uptodate;

        public ConfigManager mgr_cfg;

        private Dictionary<int, string> downloadable_files = new Dictionary<int, string>();

        private BackgroundWorker bgWorker_check;

        private BackgroundWorker bgWorker_hashchecker;

        private BackgroundWorker bgWorker_updater;

        private BackgroundWorker bgWorker_downloadupdate;

        private Process process_Game;

        private int cabalmain_build;

        public MainWindow()
        {
            InitializeComponent();
            base.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = ViewModel;
            bgWorker_check = new BackgroundWorker();
            bgWorker_check.DoWork += bgWorker_check_DoWork;
            bgWorker_check.RunWorkerCompleted += bgWorker_check_Completted;
            bgWorker_check.RunWorkerAsync();
            ViewModel.ReportStatus.Report("Connecting to the update server...");
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            string.Format(CultureInfo.InvariantCulture, "Launcher Version {0}.{1}.{2} (r{3})", version.Major, version.Minor, version.Build, version.Revision);
        }

        private void web_browser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            e.Cancel = true;
            if (e.Uri.ToString().EndsWith("#"))
            {
                return;
            }
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = e.Uri.ToString()
            };
            try
            {
                Process.Start(startInfo);
            }
            catch
            {
            }
        }

        private void bgWorker_check_DoWork(object sender, DoWorkEventArgs e)
        {
            WebClient webClient = new WebClient();
            webClient.Proxy = null;
            try
            {
                webClient.DownloadFile(new Uri(launcher_hostname + "\\" + launcher_ininame), dir_temp + launcher_ininame);
                Console.WriteLine("Downloading " + launcher_ininame);
                is_working = true;
            }
            catch (Exception)
            {
                Console.WriteLine("Error while downloading " + launcher_ininame);
                is_working = false;
            }
        }

        private void bgWorker_check_Completted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (File.Exists(dir_temp + launcher_ininame) && is_working)
            {
                mgr_cfg = new ConfigManager(dir_temp, launcher_ininame, remote: true);
                mgr_cfg.LoadRemote();
                Console.WriteLine("Loaded " + launcher_ininame);
                bgWorker_updater = new BackgroundWorker();
                bgWorker_updater.DoWork += bgWorker_updater_DoWork;
                bgWorker_updater.RunWorkerCompleted += bgWorker_updater_Completted;
                bgWorker_updater.RunWorkerAsync();
            }
            else
            {
                Console.WriteLine("Error loading " + launcher_ininame);
                lbl_status.Content = "Error while connecting to the update server...";
            }
        }

        private void bgWorker_updater_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(dir_current + "\\" + client_cabalmain);
                cabalmain_build = versionInfo.FilePrivatePart;
            }
            catch
            {
                cabalmain_build = 0;
            }
            Console.WriteLine(client_cabalmain + " Build: {0}", cabalmain_build);
            if (mgr_cfg.ConfigRemote.Settings.CabalHash != App.GetMd5HashFromFile("AutoP.exe"))
            {
                ViewModel.FileDownloader.SetSource(client_hostname + "client/AutoP.exe");
                ViewModel.FileDownloader.SetTarget("AutoP.exe");
                ViewModel.FileDownloader.Download();
                while (ViewModel.FileDownloader.IsRunning())
                {
                    Thread.Sleep(50);
                    ViewModel.ReportStatus.Report("Downloading required files...");
                }
                uptodate = true;
            }
            else if (App.GetMd5HashFromFile(client_cabalmain) != mgr_cfg.ConfigRemote.Settings.CabalMainHash)
            {
                ViewModel.FileDownloader.SetSource(client_hostname + "client/" + client_cabalmain);
                ViewModel.FileDownloader.SetTarget(client_cabalmain);
                ViewModel.FileDownloader.Download();
                while (ViewModel.FileDownloader.IsRunning())
                {
                    Thread.Sleep(50);
                }
                uptodate = true;
            }
            else
            {
                uptodate = true;
            }
        }

        private void bgWorker_updater_Completted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!uptodate)
            {
                bgWorker_downloadupdate = new BackgroundWorker();
                bgWorker_downloadupdate.DoWork += bgWorker_downloadupdate_DoWork;
                bgWorker_downloadupdate.RunWorkerCompleted += bgWorker_downloadupdate_RunWorkerCompleted;
                bgWorker_downloadupdate.RunWorkerAsync();
                return;
            }
            pb_downloadPercentTxt.Text = "";
            pb_downloadPercentTxt2.Text = "AutoP";
            using (StreamReader streamReader = new StreamReader("version.dat"))
            {
                pb_downloadPercentTxt3.Text = "| " + streamReader.ReadToEnd();
            }
            using (StreamReader streamReader2 = new StreamReader("version.dat"))
            {
                pb_downloadPercentTxt4.Text = "CLIENT " + streamReader2.ReadToEnd();
            }
            ViewModel.ReportStatus.Report("Update completed. Finished installation...");
            ViewModel.ReportProgess.Progess(100, 100);
            ViewModel.FileDownloader.Percentage = 100;
            bgWorker_hashchecker = new BackgroundWorker();
            bgWorker_hashchecker.DoWork += bgWorker_hashchecker_DoWork;
            bgWorker_hashchecker.RunWorkerCompleted += bgWorker_hashchecker_Completted;
            bgWorker_hashchecker.RunWorkerAsync();
        }

        private void bgWorker_downloadupdate_DoWork(object sender, DoWorkEventArgs e)
        {
            int num = 0;
            if (mgr_cfg.ConfigRemote.Updates.count > 0)
            {
                foreach (RemoteSettings.CUpdate item in mgr_cfg.ConfigRemote.Updates.Update)
                {
                    Console.WriteLine("Current step: {0}  start_from: {1}", item.version, num);
                    if (item.version > cabalmain_build)
                    {
                        Console.WriteLine("Update Version: {0}  File: {1}  Hash: {2}", item.version, item.file, item.hash);
                        ViewModel.FileDownloader.SetSource(launcher_hostname + item.file);
                        ViewModel.FileDownloader.SetTarget(dir_temp + item.file);
                        ViewModel.FileDownloader.Download();
                        ViewModel.ReportProgess.Progess(0, 100);
                        while (ViewModel.FileDownloader.IsRunning())
                        {
                            ViewModel.ReportStatus.Report("Downloading required files...");
                            Thread.Sleep(50);
                        }
                        if (!ViewModel.FileDownloader.Completted() || !(item.hash == App.GetMd5HashFromFile(dir_temp + item.file)))
                        {
                            Console.WriteLine("Download failed...");
                            uptodate = false;
                            break;
                        }
                        Console.WriteLine("Extracting: " + item.file);
                        ViewModel.FileExtractor.SetSource(dir_temp + item.file);
                        ViewModel.FileExtractor.SetTarget(dir_current);
                        ViewModel.FileExtractor.SetPassword(launcher_archive);
                        ViewModel.FileExtractor.Extract();
                        while (ViewModel.FileExtractor.IsRunning())
                        {
                            Thread.Sleep(50);
                            ViewModel.ReportProgess.Progess(ViewModel.FileExtractor.Percentage, 100);
                            ViewModel.ReportStatus.Report("Installing files...");
                        }
                        Console.WriteLine("Extracting completed...");
                        try
                        {
                            File.Delete(dir_temp + item.file);
                        }
                        catch
                        {
                        }
                        uptodate = true;
                        num++;
                    }
                }
                return;
            }
            uptodate = false;
        }

        private void bgWorker_downloadupdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (uptodate)
            {
                pb_downloadPercentTxt.Text = "";
                pb_downloadPercentTxt2.Text = "AutoP";
                using (StreamReader streamReader = new StreamReader("version.dat"))
                {
                    pb_downloadPercentTxt3.Text = "| " + streamReader.ReadToEnd();
                }
                using (StreamReader streamReader2 = new StreamReader("version.dat"))
                {
                    pb_downloadPercentTxt4.Text = "CLIENT " + streamReader2.ReadToEnd();
                }
                ViewModel.ReportStatus.Report("Update completed. Finished installation...");
                ViewModel.ReportProgess.Progess(100, 100);
                ViewModel.FileDownloader.Percentage = 100;
                bgWorker_hashchecker = new BackgroundWorker();
                bgWorker_hashchecker.DoWork += bgWorker_hashchecker_DoWork;
                bgWorker_hashchecker.RunWorkerCompleted += bgWorker_hashchecker_Completted;
                bgWorker_hashchecker.RunWorkerAsync();
            }
            else
            {
                ViewModel.ReportStatus.Report("Update failed.");
            }
        }

        private void bgWorker_hashchecker_DoWork(object sender, DoWorkEventArgs e)
        {
            int num = 1;
            downloadable_files.Clear();
            foreach (RemoteSettings.CHash item in mgr_cfg.ConfigRemote.Hashes.Hash)
            {
                if (item.hash != App.GetMd5HashFromFile(item.file))
                {
                    downloadable_files.Add(num, item.file);
                    Console.WriteLine("Index: {0}  File: {1}", num, item.file);
                    ViewModel.FileDownloader.SetSource(client_hostname + "client/" + item.file);
                    ViewModel.FileDownloader.SetTarget(item.file);
                    ViewModel.FileDownloader.Download();
                    while (ViewModel.FileDownloader.IsRunning())
                    {
                        Thread.Sleep(50);
                    }
                }
                ViewModel.ReportProgess.Progess(num, mgr_cfg.ConfigRemote.Hashes.count);
                ViewModel.ReportStatus.Report("Checking required files " + num + " of " + mgr_cfg.ConfigRemote.Hashes.count);
                num++;
            }
            if (num == mgr_cfg.ConfigRemote.Hashes.count)
            {
                uptodate = false;
            }
            else if (App.GetMd5HashFromFile(client_cabalmain) != mgr_cfg.ConfigRemote.Settings.CabalMainHash)
            {
                ViewModel.FileDownloader.SetSource(client_hostname + "client/" + client_cabalmain);
                ViewModel.FileDownloader.SetTarget(client_cabalmain);
                ViewModel.FileDownloader.Download();
                while (ViewModel.FileDownloader.IsRunning())
                {
                    Thread.Sleep(50);
                }
                uptodate = true;
            }
            else
            {
                uptodate = true;
            }
        }

        private void bgWorker_hashchecker_Completted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!uptodate)
            {
                ViewModel.ReportStatus.Report("Update failed.");
                Console.WriteLine("Update failed...");
                return;
            }
            pb_downloadPercentTxt.Text = "";
            pb_downloadPercentTxt2.Text = "AutoP";
            using (StreamReader streamReader = new StreamReader("version.dat"))
            {
                pb_downloadPercentTxt3.Text = "| " + streamReader.ReadToEnd();
            }
            using (StreamReader streamReader2 = new StreamReader("version.dat"))
            {
                pb_downloadPercentTxt4.Text = "CLIENT " + streamReader2.ReadToEnd();
            }
            ViewModel.ReportStatus.Report("Update completed. Finished installation...");
            ViewModel.ReportProgess.Progess(100, 100);
            ViewModel.FileDownloader.Percentage = 100;
            btn_start.IsEnabled = true;
            btn_start2.IsEnabled = true;
            btn_option.IsEnabled = false;
            btn_check.IsEnabled = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            process_Game = new Process();
            process_Game.StartInfo.FileName = dir_current + client_cabalmain;
            process_Game.StartInfo.WorkingDirectory = dir_current;
            process_Game.StartInfo.UseShellExecute = false;
            process_Game.StartInfo.Arguments = "blacknull";
            process_Game.Start();
            Application.Current.Shutdown(110);
        }

        private void btn_start2_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(110);
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(110);
        }

        private void RootGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void btn_option_Click(object sender, RoutedEventArgs e)
        {
        }

        private void button_web_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://zeroonlinevn.com");
        }

        private void btn_check_Click(object sender, RoutedEventArgs e)
        {
            int num = 1;
            downloadable_files.Clear();
            foreach (RemoteSettings.CHash item in mgr_cfg.ConfigRemote.Hashes.Hash)
            {
                if (item.hash != App.GetMd5HashFromFile(item.file))
                {
                    downloadable_files.Add(num, item.file);
                    Console.WriteLine("Index: {0}  File: {1}", num, item.file);
                    ViewModel.FileDownloader.SetSource(client_hostname + "client/" + item.file);
                    ViewModel.FileDownloader.SetTarget(item.file);
                    if (!uptodate)
                    {
                        bgWorker_downloadupdate = new BackgroundWorker();
                        bgWorker_downloadupdate.DoWork += bgWorker_downloadupdate_DoWork;
                        bgWorker_downloadupdate.RunWorkerCompleted += bgWorker_downloadupdate_RunWorkerCompleted;
                        bgWorker_downloadupdate.RunWorkerAsync();
                    }
                    else
                    {
                        pb_downloadPercentTxt.Text = "";
                        pb_downloadPercentTxt2.Text = "AutoP";
                        using (StreamReader streamReader = new StreamReader("version.dat"))
                        {
                            pb_downloadPercentTxt3.Text = "| " + streamReader.ReadToEnd();
                        }
                        using (StreamReader streamReader2 = new StreamReader("version.dat"))
                        {
                            pb_downloadPercentTxt4.Text = "CLIENT " + streamReader2.ReadToEnd();
                        }
                        ViewModel.ReportStatus.Report("Update completed. Finished installation...");
                        ViewModel.ReportProgess.Progess(100, 100);
                        ViewModel.FileDownloader.Percentage = 100;
                        bgWorker_hashchecker = new BackgroundWorker();
                        bgWorker_hashchecker.DoWork += bgWorker_hashchecker_DoWork;
                        bgWorker_hashchecker.RunWorkerCompleted += bgWorker_hashchecker_Completted;
                        bgWorker_hashchecker.RunWorkerAsync();
                    }
                }
                ViewModel.ReportProgess.Progess(num, mgr_cfg.ConfigRemote.Hashes.count);
                ViewModel.ReportStatus.Report("Checking required files " + num + " of " + mgr_cfg.ConfigRemote.Hashes.count);
                num++;
            }
            if (num == mgr_cfg.ConfigRemote.Hashes.count)
            {
                uptodate = false;
            }
            else if (App.GetMd5HashFromFile(client_cabalmain) != mgr_cfg.ConfigRemote.Settings.CabalMainHash)
            {
                ViewModel.FileDownloader.SetSource(client_hostname + "client/" + client_cabalmain);
                ViewModel.FileDownloader.SetTarget(client_cabalmain);
                if (!uptodate)
                {
                    bgWorker_downloadupdate = new BackgroundWorker();
                    bgWorker_downloadupdate.DoWork += bgWorker_downloadupdate_DoWork;
                    bgWorker_downloadupdate.RunWorkerCompleted += bgWorker_downloadupdate_RunWorkerCompleted;
                    bgWorker_downloadupdate.RunWorkerAsync();
                    return;
                }
                pb_downloadPercentTxt.Text = "";
                pb_downloadPercentTxt2.Text = "AutoP";
                using (StreamReader streamReader3 = new StreamReader("version.dat"))
                {
                    pb_downloadPercentTxt3.Text = "| " + streamReader3.ReadToEnd();
                }
                using (StreamReader streamReader4 = new StreamReader("version.dat"))
                {
                    pb_downloadPercentTxt4.Text = "CLIENT " + streamReader4.ReadToEnd();
                }
                ViewModel.ReportStatus.Report("Update completed. Finished installation...");
                ViewModel.ReportProgess.Progess(100, 100);
                ViewModel.FileDownloader.Percentage = 100;
                bgWorker_hashchecker = new BackgroundWorker();
                bgWorker_hashchecker.DoWork += bgWorker_hashchecker_DoWork;
                bgWorker_hashchecker.RunWorkerCompleted += bgWorker_hashchecker_Completted;
                bgWorker_hashchecker.RunWorkerAsync();
            }
            else
            {
                uptodate = true;
            }
        }
    }
}

