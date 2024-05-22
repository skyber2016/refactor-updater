using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using ZeroOnline.Functions;
using ZeroOnline.ViewModels;

namespace ZeroOnline;

public partial class MainWindow : Window
{
	private MainViewModel ViewModel = new MainViewModel();

	public static string launcher_hostname = "https://update.zeroonlinevn.com/";

	public static string launcher_ininame = "resources.xml";

	public static string client_hostname = launcher_hostname;

	public WebClient webc = new WebClient();

	public BackgroundWorker bgWorker_check;

	public BackgroundWorker bgWorker_startdownload;

	public ConfigManager mgr_cfg;

	private static string dir_temp = Path.GetTempPath() + "\\";

	public string dir_current = Directory.GetCurrentDirectory() + "\\";

	public bool is_working;

	public bool forced;

	private Process launcher;

	public MainWindow()
	{
		InitializeComponent();
		this.DataContext = ViewModel;
		bgWorker_check = new BackgroundWorker();
		bgWorker_check.DoWork += processCheck;
		bgWorker_check.RunWorkerCompleted += processCheckResponse;
		bgWorker_check.RunWorkerAsync();
	}

	private void processCheck(object sender, DoWorkEventArgs e)
	{
		WebClient webClient = new WebClient();
		webClient.Proxy = null;
		try
		{
			webClient.DownloadFile(new Uri(launcher_hostname + launcher_ininame), dir_temp + launcher_ininame);
			is_working = true;
		}
		catch (Exception)
		{
			is_working = false;
		}
	}

	private void processCheckResponse(object sender, RunWorkerCompletedEventArgs e)
	{
		if (File.Exists(dir_temp + launcher_ininame) && is_working)
		{
			mgr_cfg = new ConfigManager(dir_temp, launcher_ininame, remote: true);
			mgr_cfg.LoadRemote();
			bgWorker_startdownload = new BackgroundWorker();
			bgWorker_startdownload.DoWork += processDownload;
			bgWorker_startdownload.RunWorkerCompleted += processDownloadResponse;
			bgWorker_startdownload.RunWorkerAsync();
		}
		else
		{
			MessageBox.Show("Error while connecting to the updater server...", "Error", MessageBoxButton.OK);
			Process.GetCurrentProcess().Kill();
		}
	}

	private void processDownload(object sender, DoWorkEventArgs e)
	{
		Process[] processesByName = Process.GetProcessesByName("Update");
		try
		{
			Process[] array = processesByName;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Kill();
			}
		}
		catch
		{
		}
		if (forced || mgr_cfg.ConfigRemote.Settings.UpdateHash != App.GetMd5HashFromFile("Update.exe"))
		{
			ViewModel.FileDownloader.SetSource(client_hostname + "/client/Update.exe");
			ViewModel.FileDownloader.SetTarget("Update.exe");
			ViewModel.FileDownloader.Download();
			while (ViewModel.FileDownloader.IsRunning())
			{
				Thread.Sleep(50);
			}
			ViewModel.FileDownloader.SetSource(client_hostname + "/client/7z.dll");
			ViewModel.FileDownloader.SetTarget("7z.dll");
			ViewModel.FileDownloader.Download();
			while (ViewModel.FileDownloader.IsRunning())
			{
				Thread.Sleep(50);
			}
			ViewModel.FileDownloader.SetSource(client_hostname + "/client/SevenZipSharp.dll");
			ViewModel.FileDownloader.SetTarget("SevenZipSharp.dll");
			ViewModel.FileDownloader.Download();
			while (ViewModel.FileDownloader.IsRunning())
			{
				Thread.Sleep(50);
			}
			ViewModel.FileDownloader.SetSource(client_hostname + "/client/System.Windows.Interactivity.dll");
			ViewModel.FileDownloader.SetTarget("System.Windows.Interactivity.dll");
			ViewModel.FileDownloader.Download();
			while (ViewModel.FileDownloader.IsRunning())
			{
				Thread.Sleep(50);
			}
		}
	}

	private void processDownloadResponse(object sender, RunWorkerCompletedEventArgs e)
	{
		if (File.Exists(dir_temp + launcher_ininame))
		{
			File.Delete(dir_temp + launcher_ininame);
		}
		launcher = new Process();
		launcher.StartInfo.FileName = dir_current + "Update.exe";
		launcher.StartInfo.WorkingDirectory = dir_current;
		launcher.StartInfo.Arguments = "mmoparadox";
		launcher.Start();
		Process.GetCurrentProcess().Kill();
	}
}
