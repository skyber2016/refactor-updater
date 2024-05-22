using System.Xml.Serialization;

namespace Update.Classes.Functions;

[XmlRoot("ConfigTemplate")]
public class ConfigTemplate
{
	private int cabalmainbuild;

	private int launcherrevision;

	private string updatehash = "";

	private string cabalhash = "";

	private string cabalmainhash = "";

	private string updateversion = "";

	[XmlElement("UpdateHash")]
	public string UpdateHash
	{
		get
		{
			return updatehash;
		}
		set
		{
			updatehash = value;
		}
	}

	[XmlElement("CabalHash")]
	public string CabalHash
	{
		get
		{
			return cabalhash;
		}
		set
		{
			cabalhash = value;
		}
	}

	[XmlElement("UpdateVersion")]
	public string UpdateVersion
	{
		get
		{
			return updateversion;
		}
		set
		{
			updateversion = value;
		}
	}

	[XmlElement("LauncherRevision")]
	public int LauncherRevision
	{
		get
		{
			return launcherrevision;
		}
		set
		{
			launcherrevision = value;
		}
	}

	[XmlElement("CabalMainHash")]
	public string CabalMainHash
	{
		get
		{
			return cabalmainhash;
		}
		set
		{
			cabalmainhash = value;
		}
	}

	[XmlElement("CabalMainBuild")]
	public int CabalMainBuild
	{
		get
		{
			return cabalmainbuild;
		}
		set
		{
			cabalmainbuild = value;
		}
	}
}
