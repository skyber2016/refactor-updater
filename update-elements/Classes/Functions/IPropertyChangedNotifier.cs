using System.ComponentModel;

namespace Update.Classes.Functions;

public interface IPropertyChangedNotifier : INotifyPropertyChanged
{
	void NotifyPropertyChanged(string propertyName);
}
