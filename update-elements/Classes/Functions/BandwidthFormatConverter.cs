using System;
using System.Globalization;
using System.Windows.Data;

namespace Update.Classes.Functions;

[ValueConversion(typeof(object), typeof(string))]
public class BandwidthFormatConverter : BaseConverter, IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return $"{value:0}" + " (KB/s)";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return null;
	}
}
