using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Update.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("Update.Properties.Resources", typeof(Resources).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	internal static Bitmap BACKMAIN => (Bitmap)ResourceManager.GetObject("BACKMAIN", resourceCulture);

	internal static Bitmap close_hover => (Bitmap)ResourceManager.GetObject("close_hover", resourceCulture);

	internal static Bitmap close_normal => (Bitmap)ResourceManager.GetObject("close_normal", resourceCulture);

	internal static byte[] main => (byte[])ResourceManager.GetObject("main", resourceCulture);

	internal static byte[] mainEX => (byte[])ResourceManager.GetObject("mainEX", resourceCulture);

	internal static Bitmap OPTIONCLICK => (Bitmap)ResourceManager.GetObject("OPTIONCLICK", resourceCulture);

	internal static Bitmap OPTIONNORMAL => (Bitmap)ResourceManager.GetObject("OPTIONNORMAL", resourceCulture);

	internal static Bitmap OPTIONOVER => (Bitmap)ResourceManager.GetObject("OPTIONOVER", resourceCulture);

	internal static Bitmap STARTCLICK => (Bitmap)ResourceManager.GetObject("STARTCLICK", resourceCulture);

	internal static Bitmap STARTNORMAL => (Bitmap)ResourceManager.GetObject("STARTNORMAL", resourceCulture);

	internal static Bitmap STARTOVER => (Bitmap)ResourceManager.GetObject("STARTOVER", resourceCulture);

	internal Resources()
	{
	}
}
