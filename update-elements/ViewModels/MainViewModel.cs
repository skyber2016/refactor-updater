using Update.Classes.Functions;

namespace Update.ViewModels;

internal class MainViewModel : PropertyChangedBase
{
	private FileDownloader downloader;

	private FileExtractor extractor;

	private ReportProgess reportprogress;

	private ReportStatus reportstatus;

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

	public FileExtractor FileExtractor
	{
		get
		{
			if (extractor == null)
			{
				extractor = new FileExtractor();
			}
			return extractor;
		}
		set
		{
			extractor = value;
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

	public ReportStatus ReportStatus
	{
		get
		{
			if (reportstatus == null)
			{
				reportstatus = new ReportStatus();
			}
			return reportstatus;
		}
		set
		{
			reportstatus = value;
		}
	}
}
