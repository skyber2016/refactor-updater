using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Update;

public class App : Application
{
	public class LauncherException : Exception
	{
		public LauncherException(Exception innerException)
		{
			_ = innerException.Message + Environment.NewLine + innerException.StackTrace;
			MessageBox.Show("Failed to connect to the server. Please try again." + Environment.NewLine, "Error", MessageBoxButton.OK);
			Process.GetCurrentProcess().Kill();
		}
	}

	public static string xtrap_server_ip = "";

	private const string appname = "Update";

	[DllImport("dnsapi.dll")]
	private static extern uint DnsFlushResolverCache();

	protected override void OnStartup(StartupEventArgs e)
	{
		CheckInstance();
		CheckSecurity();
		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
		try
		{
			DnsFlushResolverCache();
			xtrap_server_ip = Dns.GetHostAddresses(xtrap_server_ip)[0].ToString();
		}
		catch (Exception innerException)
		{
			throw new LauncherException(innerException);
		}
	}

	private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		throw new LauncherException((Exception)e.ExceptionObject);
	}

	private static void CheckSecurity()
	{
		if (!Process.GetCurrentProcess().ProcessName.Equals("Update"))
		{
			Process.GetCurrentProcess().Kill();
		}
	}

	private static void CheckInstance()
	{
		if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
		{
			Process.GetCurrentProcess().Kill();
		}
		if (Process.GetProcessesByName("AutoP").Length >= 1)
		{
			Process.GetCurrentProcess().Kill();
		}
		if (Process.GetProcessesByName("launcher").Length >= 1)
		{
			Process.GetCurrentProcess().Kill();
		}
	}

	public static string GetMd5HashFromFile(string fileName)
	{
		if (File.Exists(fileName))
		{
			FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
			byte[] array = new MD5CryptoServiceProvider().ComputeHash(fileStream);
			fileStream.Close();
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x2"));
			}
			return stringBuilder.ToString();
		}
		return "";
	}

	public void InitializeComponent()
	{
		base.StartupUri = new Uri("Forms/MainWindow.xaml", UriKind.Relative);
	}

	[STAThread]
	public static void Main()
	{
        App app = new App();
		app.InitializeComponent();
        app.Run();
    }
}
