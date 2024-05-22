using System.ComponentModel;

namespace ZeroOnline.Functions;

public interface IPropertyChangedNotifier : INotifyPropertyChanged
{
	void NotifyPropertyChanged(string propertyName);
}
