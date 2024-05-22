using System;
using System.Windows.Markup;

namespace Update.Classes.Functions;

public abstract class BaseConverter : MarkupExtension
{
	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}
}
