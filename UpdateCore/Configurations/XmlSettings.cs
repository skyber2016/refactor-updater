using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Xml;
using System.Xml.Serialization;

namespace Update.Core
{

    [XmlRoot("RemoteSettings")]
    public class XmlSettings
    {
        public class CSettings
        {
            [XmlElement("Maintenance")]
            public bool Maintenance { get; set; } = false;

            [XmlElement("CabalHash")]
            public string CabalHash { get; set; } = string.Empty;

            [XmlElement("UpdateHash")]
            public string UpdateHash { get; set; } = string.Empty;

            [XmlElement("UpdateVersion")]
            public string UpdateVersion { get; set; } = "0";

            [XmlElement("UpdateRevision")]
            public int UpdateRevision { get; set; } = 0;

            [XmlElement("CabalMainHash")]
            public string CabalMainHash { get; set; } = string.Empty;

            [XmlElement("CabalMainBuild")]
            public int CabalMainBuild { get; set; } = 0;

            [XmlElement("CabalMainConstructor")]
            public string CabalMainConstructor { get; set; }
        }

        public class CHashes
        {
            public CHashes()
            {
                this.Hash = new List<CHash>();
                this.Count = 0;
            }
            [XmlAttribute("count")]
            public int Count { get; set; }

            [XmlElement("Hash")]
            public List<CHash> Hash { get; set; }

            public void Append(CHash data)
            {
                var item = Hash.FirstOrDefault(x=> x.File == data.File);
                if(item != null)
                {
                    this.Hash.Remove(item);
                }
                this.Hash.Add(data);
                this.Count = this.Hash.Count;
            }
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
            public string File { get; set; }

            [XmlText]
            public string Hash { get; set; }
        }

        public class CUpdates
        {
            public CUpdates()
            {
                this.Update = new List<CUpdate>();
                this.Count = 0;
            }
            [XmlAttribute("count")]
            public int Count { get; set; }

            [XmlElement("Update")]
            public List<CUpdate> Update { get; set; }

            public void Append(CUpdate data)
            {
                var item = Update.FirstOrDefault(x => x.File == data.File);
                if (item != null)
                {
                    this.Update.Remove(item);
                }
                this.Update.Add(data);
                this.Count = this.Update.Count;
            }
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
            public string File { get; set; }

            [XmlAttribute("version")]
            public int Version { get; set; }

            [XmlAttribute("hash")]
            public string Hash { get; set; }
        }

        [XmlElement("Settings")]
        public CSettings Settings { get; set; } = new CSettings();

        [XmlElement("Hashes")]
        public CHashes Hashes { get; set; } = new CHashes();

        [XmlElement("Updates")]
        public CUpdates Updates { get; set; } = new CUpdates();
    }
}
