using System.Collections.Generic;
using System.Xml.Serialization;

namespace Update.Classes.Functions;

[XmlRoot("RemoteSettings")]
public class RemoteSettings
{
	public class CSettings
	{
		[XmlElement("Maintenance")]
		public bool Maintenance { get; set; }

		[XmlElement("CabalHash")]
		public string CabalHash { get; set; }

		[XmlElement("UpdateHash")]
		public string UpdateHash { get; set; }

		[XmlElement("UpdateVersion")]
		public string UpdateVersion { get; set; }

		[XmlElement("UpdateRevision")]
		public int UpdateRevision { get; set; }

		[XmlElement("CabalMainHash")]
		public string CabalMainHash { get; set; }

		[XmlElement("CabalMainBuild")]
		public int CabalMainBuild { get; set; }

		[XmlElement("CabalMainConstructor")]
		public string CabalMainConstructor { get; set; }
	}

	public class CHashes
	{
		[XmlAttribute("count")]
		public int count { get; set; }

		[XmlElement("Hash")]
		public List<CHash> Hash { get; set; }
	}

	[XmlType(AnonymousType = true)]
	public class CHashData
	{
		[XmlElement("Hash")]
		public List<CHash> Hash { get; set; }

		public CHashData()
		{
			Hash = new List<CHash>();
		}
	}

	public class CHash
	{
		[XmlAttribute("file")]
		public string file { get; set; }

		[XmlText]
		public string hash { get; set; }
	}

	public class CUpdates
	{
		[XmlAttribute("count")]
		public int count { get; set; }

		[XmlElement("Update")]
		public List<CUpdate> Update { get; set; }
	}

	[XmlType(AnonymousType = true)]
	public class CUpdateData
	{
		[XmlElement("Update")]
		public List<CUpdate> Update { get; set; }

		public CUpdateData()
		{
			Update = new List<CUpdate>();
		}
	}

	public class CUpdate
	{
		[XmlAttribute("file")]
		public string file { get; set; }

		[XmlAttribute("version")]
		public int version { get; set; }

		[XmlAttribute("hash")]
		public string hash { get; set; }
	}

	[XmlElement("Settings")]
	public CSettings Settings { get; set; }

	[XmlElement("Hashes")]
	public CHashes Hashes { get; set; }

	[XmlElement("Updates")]
	public CUpdates Updates { get; set; }
}
