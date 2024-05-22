using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Update.Classes.Functions;

public class ConfigManager
{
	public ConfigTemplate ConfigLocal;

	public RemoteSettings ConfigRemote;

	private XmlSerializer xmlSerializer;

	private string Path;

	private string File;

	private bool Remote;

	public ConfigManager(string path, string file, bool remote)
	{
		Path = path;
		File = file;
		Remote = remote;
		if (remote)
		{
			ConfigRemote = new RemoteSettings();
			xmlSerializer = new XmlSerializer(typeof(RemoteSettings));
		}
		else if (!remote)
		{
			ConfigLocal = new ConfigTemplate();
			xmlSerializer = new XmlSerializer(typeof(ConfigTemplate));
		}
	}

	public string Display()
	{
		StringWriter stringWriter = new StringWriter();
		XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter)
		{
			Formatting = Formatting.Indented
		};
		if (Remote)
		{
			xmlSerializer.Serialize(xmlWriter, ConfigRemote);
		}
		else
		{
			xmlSerializer.Serialize(xmlWriter, ConfigLocal);
		}
		return stringWriter.ToString();
	}

	public bool Load()
	{
		try
		{
			if (System.IO.File.Exists(Path + "\\" + File))
			{
				StreamReader streamReader = new StreamReader(Path + "\\" + File);
				TextReader textReader = new StringReader(Crypto.DecryptStringAES(streamReader.ReadToEnd(), "0l}i{HE%-6QhfnYbQ1BxXxtVDCLQB/oY"));
				ConfigLocal = (ConfigTemplate)xmlSerializer.Deserialize(textReader);
				streamReader.Close();
				Console.WriteLine(File + " loaded.");
			}
			else
			{
				Save();
			}
			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine("Failed to load " + File + "! " + ex);
		}
		return false;
	}

	public bool Save()
	{
		try
		{
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlSerializer.Serialize(xmlTextWriter, ConfigLocal);
			StreamWriter streamWriter = new StreamWriter(Path + "\\" + File);
			streamWriter.Write(Crypto.EncryptStringAES(stringWriter.ToString(), "0l}i{HE%-6QhfnYbQ1BxXxtVDCLQB/oY"));
			streamWriter.Close();
			Console.WriteLine(File + " saved.");
			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine("Failed to save " + File + "! " + ex.Message);
		}
		return false;
	}

	public bool LoadRemote()
	{
		try
		{
			if (System.IO.File.Exists(Path + "\\" + File))
			{
				StreamReader streamReader = new StreamReader(Path + "\\" + File);
				TextReader textReader = new StringReader(streamReader.ReadToEnd());
				ConfigRemote = (RemoteSettings)xmlSerializer.Deserialize(textReader);
				streamReader.Close();
				Console.WriteLine(File + " loaded.");
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			Console.WriteLine("Failed to load " + File + "! " + ex);
		}
		return false;
	}

	public bool SaveRemote()
	{
		try
		{
			StringWriter stringWriter = new StringWriter();
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlSerializer.Serialize(xmlTextWriter, ConfigRemote);
			StreamWriter streamWriter = new StreamWriter(Path + "\\" + File);
			streamWriter.Write(stringWriter.ToString());
			streamWriter.Close();
			Console.WriteLine(File + " saved.");
			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine("Failed to save " + File + "! " + ex.Message);
		}
		return false;
	}
}
