using System.Collections.Generic;
using System.Xml.Serialization;

namespace ZeroOnline.Functions;

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

	[XmlElement("Settings")]
	public CSettings Settings { get; set; }

	[XmlElement("Hashes")]
	public CHashes Hashes { get; set; }
}
