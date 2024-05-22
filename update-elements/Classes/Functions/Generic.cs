using System;
using System.IO;
using System.Reflection;
using Update.Forms;

namespace Update.Classes.Functions;

internal static class Generic
{
	public static void CheckHosts(string server_ip)
	{
		string path = Path.GetPathRoot(Environment.SystemDirectory) + "Windows\\System32\\drivers\\etc\\hosts";
		string text = "";
		bool flag = true;
		string[] array = File.ReadAllLines(path);
		foreach (string text2 in array)
		{
			if (!text2.Contains("xtrap"))
			{
				text = text + text2 + "\r\n";
			}
			else if (text2.Contains("#"))
			{
				text = text + text2 + "\r\n";
			}
			else if (text2.Contains(server_ip) & text2.Contains("xtrap.cabalonline.com.br"))
			{
				text = text + text2 + "\r\n";
				flag = false;
			}
		}
		if (flag)
		{
			string value = text + server_ip + "\txtrap.cabalonline.com.br\r\n";
			Console.WriteLine(value);
			StreamWriter streamWriter = new StreamWriter(path);
			streamWriter.WriteLine(value);
			streamWriter.Close();
		}
	}

	public static void CheckMainFiles()
	{
		CabalConfigEditor.SettingsHandler settingsHandler = new CabalConfigEditor.SettingsHandler();
		if (!File.Exists(MainWindow.dir_current + "main.dat") || !File.Exists(MainWindow.dir_current + "mainEX.dat"))
		{
			WriteResourceToFile(Assembly.GetExecutingAssembly(), "Resources.CabalFiles.main.dat", "main.dat");
			WriteResourceToFile(Assembly.GetExecutingAssembly(), "Resources.CabalFiles.mainEX.dat", "mainEX.dat");
		}
		settingsHandler.Open();
		settingsHandler.LanguageCode = CabalConfigEditor.Language.English;
		settingsHandler.Save();
	}

	public static void SetAttributes(string currentfolder, string folder, bool recursive)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(currentfolder + "\\" + folder);
		FileInfo[] files = directoryInfo.GetFiles();
		foreach (FileInfo obj in files)
		{
			File.SetAttributes(obj.FullName, FileAttributes.Normal);
			obj.Delete();
		}
		if (recursive)
		{
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			foreach (DirectoryInfo directoryInfo2 in directories)
			{
				SetAttributes(currentfolder, folder + "\\" + directoryInfo2.Name, recursive);
			}
		}
	}

	public static void WriteResourceToFile(Assembly targetAssembly, string resourceName, string filepath)
	{
		using Stream stream = targetAssembly.GetManifestResourceStream(targetAssembly.GetName().Name + "." + resourceName);
		if (stream == null)
		{
			throw new Exception("Cannot find embedded resource '" + resourceName + "'");
		}
		byte[] array = new byte[stream.Length];
		stream.Read(array, 0, array.Length);
		using BinaryWriter binaryWriter = new BinaryWriter(File.Open(filepath, FileMode.Create));
		binaryWriter.Write(array);
	}
}
