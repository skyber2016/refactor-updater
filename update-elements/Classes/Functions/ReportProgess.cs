namespace Update.Classes.Functions;

internal class ReportProgess : PropertyChangedBase
{
	private double percentage;

	private double percent;

	public double Percentage
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

	public void Progess(int count, int maxvalue)
	{
		percent = count * 100 / maxvalue;
		Percentage = percent;
	}
}
