using System;
using System.Globalization;
using System.Windows.Data;

namespace ZeroOnline.Functions;

[ValueConversion(typeof(object), typeof(string))]
public class StringFormatConverter : BaseConverter, IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value.ToString() == "0")
		{
			return "Downloading files...";
		}
		return "Downloading Launcher files (" + value.ToString() + "%)";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return null;
	}
}
