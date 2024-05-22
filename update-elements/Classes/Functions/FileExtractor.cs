using System;
using System.ComponentModel;
using System.IO;
using SevenZip;

namespace Update.Classes.Functions;

internal class FileExtractor : PropertyChangedBase
{
	private BackgroundWorker bgWorker = new BackgroundWorker();

	private string source;

	private string target;

	private string password;

	private int percentage;

	private bool running;

	private string dir_current = Directory.GetCurrentDirectory() + "\\";

	public int Percentage
	{
		get
		{
			return percentage;
		}
		set
		{
			NotifyPropertyChanging("Percentage");
			percentage = value;
			NotifyPropertyChanged("Percentage");
		}
	}

	public void SetPassword(string pass)
	{
		password = pass;
	}

	public void SetSource(string url)
	{
		source = url;
	}

	public void SetTarget(string fileuri)
	{
		target = fileuri;
	}

	public bool IsRunning()
	{
		return running;
	}

	public FileExtractor()
	{
		SevenZipBase.SetLibraryPath(dir_current + "7z.dll");
	}

	public void Extract()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		running = true;
		Percentage = 0;
		SevenZipExtractor val = ((password == null) ? new SevenZipExtractor(source) : new SevenZipExtractor(source, password));
		val.Extracting += extr_Extracting;
		val.FileExtractionStarted += extr_FileExtractionStarted;
		val.FileExists += extr_FileExists;
		val.ExtractionFinished += extr_ExtractionFinished;
		val.BeginExtractArchive(target);
	}

	private void extr_ExtractionFinished(object sender, EventArgs e)
	{
		running = false;
	}

	private void extr_FileExists(object sender, FileOverwriteEventArgs e)
	{
		try
		{
			File.SetAttributes(e.FileName, FileAttributes.Normal);
		}
		catch
		{
		}
	}

	private void extr_FileExtractionStarted(object sender, FileInfoEventArgs e)
	{
		running = true;
	}

	private void extr_Extracting(object sender, ProgressEventArgs e)
	{
		Percentage += e.PercentDelta;
	}
}
