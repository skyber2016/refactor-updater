using ZeroOnline.Functions;

namespace ZeroOnline.ViewModels;

internal class MainViewModel : PropertyChangedBase
{
	private FileDownloader downloader;

	private ReportProgess reportprogress;

	public FileDownloader FileDownloader
	{
		get
		{
			if (downloader == null)
			{
				downloader = new FileDownloader();
			}
			return downloader;
		}
		set
		{
			downloader = value;
		}
	}

	public ReportProgess ReportProgess
	{
		get
		{
			if (reportprogress == null)
			{
				reportprogress = new ReportProgess();
			}
			return reportprogress;
		}
		set
		{
			reportprogress = value;
		}
	}
}
