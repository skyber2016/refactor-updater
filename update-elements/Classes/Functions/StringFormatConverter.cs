using System;
using System.Globalization;
using System.Windows.Data;

namespace Update.Classes.Functions;

[ValueConversion(typeof(object), typeof(string))]
public class StringFormatConverter : BaseConverter, IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value.ToString() + "%";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return null;
	}
}
