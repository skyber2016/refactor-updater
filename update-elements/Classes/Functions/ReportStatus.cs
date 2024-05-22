namespace Update.Classes.Functions;

internal class ReportStatus : PropertyChangedBase
{
	private string status_text;

	public string Text
	{
		get
		{
			return status_text;
		}
		set
		{
			NotifyPropertyChanging("Text");
			status_text = value;
			NotifyPropertyChanged("Text");
		}
	}

	public void Report(string text)
	{
		Text = text;
	}
}
