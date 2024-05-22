using System;
using System.Globalization;
using System.Windows.Data;

namespace Update.Classes.Functions;

[ValueConversion(typeof(object), typeof(string))]
public class SizeFormatConverter : BaseConverter, IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		long num = System.Convert.ToInt64(value);
		if (num >= 1073741824)
		{
			return $"{num / 1073741824:0}" + " GB";
		}
		if (num >= 1048576)
		{
			return $"{num / 1048576:0}" + " MB";
		}
		if (num >= 1024)
		{
			return $"{num / 1024:0}" + " KB";
		}
		return $"{num:0}" + " B";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return null;
	}
}
