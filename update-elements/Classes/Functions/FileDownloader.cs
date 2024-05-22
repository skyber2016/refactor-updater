using System;
using System.ComponentModel;
using System.IO;
using System.Net;

namespace Update.Classes.Functions;

internal class FileDownloader : PropertyChangedBase
{
	private BackgroundWorker bgWorker = new BackgroundWorker();

	private DateTime startTime;

	private string source;

	private string target;

	private double avgBandwidth;

	private double crntBandwidth;

	private int percentage;

	private long file_size;

	private long file_downloaded;

	private bool running;

	private bool completted;

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

	public double AverageBandwidth
	{
		get
		{
			return avgBandwidth;
		}
		private set
		{
			NotifyPropertyChanging("AverageBandwidth");
			avgBandwidth = value;
			NotifyPropertyChanged("AverageBandwidth");
		}
	}

	public double CurrentBandwidth
	{
		get
		{
			return crntBandwidth;
		}
		private set
		{
			NotifyPropertyChanging("CurrentBandwidth");
			crntBandwidth = value;
			NotifyPropertyChanged("CurrentBandwidth");
		}
	}

	public long Downloaded
	{
		get
		{
			return file_downloaded;
		}
		private set
		{
			NotifyPropertyChanging("Downloaded");
			file_downloaded = value;
			NotifyPropertyChanged("Downloaded");
		}
	}

	public long DownloadSize
	{
		get
		{
			return file_size;
		}
		private set
		{
			NotifyPropertyChanging("DownloadSize");
			file_size = value;
			NotifyPropertyChanged("DownloadSize");
		}
	}

	public void SetSource(string url)
	{
		source = url;
	}

	public void SetTarget(string fileuri)
	{
		target = fileuri;
	}

	public void SupportCancellation(bool value)
	{
		bgWorker.WorkerSupportsCancellation = value;
	}

	public bool IsRunning()
	{
		return running;
	}

	public bool Completted()
	{
		return completted;
	}

	public FileDownloader()
	{
		bgWorker.WorkerReportsProgress = true;
		bgWorker.WorkerSupportsCancellation = true;
		bgWorker.ProgressChanged += bgWorker_ProgressChanged;
		bgWorker.DoWork += bgWorker_DoWork;
		bgWorker.RunWorkerCompleted += bgWorker_RunWorkerCompleted;
	}

	public void Download()
	{
		running = true;
		Percentage = 0;
		startTime = DateTime.Now;
		CurrentBandwidth = 0.0;
		AverageBandwidth = 0.0;
		bgWorker.RunWorkerAsync();
	}

	public void Cancel()
	{
		if (running)
		{
			bgWorker.CancelAsync();
			running = false;
		}
	}

	private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		DateTime now = DateTime.Now;
		DateTime now2 = DateTime.Now;
		DateTime now3 = DateTime.Now;
		BandwidthList<double> bandwidthList = new BandwidthList<double>(10);
		double num = 0.0;
		double num2 = 0.0;
		if (bgWorker.CancellationPending)
		{
			e.Cancel = true;
		}
		WebResponse response;
		try
		{
			WebRequest webRequest = WebRequest.Create(source);
			webRequest.Proxy = new WebProxy();
			response = webRequest.GetResponse();
		}
		catch
		{
			Console.WriteLine("FileDownloader | Download failed from source: " + source);
			running = false;
			completted = false;
			return;
		}
		try
		{
			if (!Directory.Exists(target.Remove(target.LastIndexOf("\\"))))
			{
				Directory.CreateDirectory(target.Remove(target.LastIndexOf("\\")));
			}
		}
		catch (Exception)
		{
		}
		long contentLength = response.ContentLength;
		using Stream stream = response.GetResponseStream();
		using Stream stream2 = File.Create(target);
		int num3 = 4096;
		byte[] buffer = new byte[num3];
		while (true)
		{
			if ((DateTime.Now - startTime).TotalSeconds > 20.0 && num == 0.0)
			{
				throw new ApplicationException("FileDownloader | Download timed out!");
			}
			int num4 = stream.Read(buffer, 0, num3);
			if (num4 == 0)
			{
				break;
			}
			try
			{
				stream2.Write(buffer, 0, num4);
			}
			catch (Exception)
			{
			}
			num += (double)num4;
			num2 += (double)num4;
			int num5 = ((contentLength >= 10240) ? 35 : 0);
			if ((now - now3).TotalMilliseconds >= (double)num5)
			{
				double value = num / (double)contentLength * 100.0;
				bgWorker.ReportProgress(Convert.ToInt32(value));
				Downloaded = Convert.ToInt64(num);
				DownloadSize = contentLength;
				now3 = DateTime.Now;
			}
			if ((now - now2).TotalSeconds >= 1.0)
			{
				TimeSpan timeSpan = now - now2;
				CurrentBandwidth = num2 / 1024.0 / timeSpan.TotalSeconds;
				bandwidthList.Add(CurrentBandwidth);
				AverageBandwidth = bandwidthList.CalculateAverage();
				now2 = DateTime.Now;
				num2 = 0.0;
			}
			now = DateTime.Now;
		}
		completted = true;
		bgWorker.ReportProgress(100);
		stream2.Close();
		stream.Close();
	}

	private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		Percentage = e.ProgressPercentage;
	}

	private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		running = false;
		CurrentBandwidth = 0.0;
		AverageBandwidth = 0.0;
	}
}
