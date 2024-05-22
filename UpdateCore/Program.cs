using Microsoft.Extensions.Configuration;
using ShellProgressBar;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UpdateCore.Services;

namespace Update.Core
{
    public static class Program
    {
        private static IConfiguration AppSetting => ConfigurationManager.Settings;
        private static string ResourcePath = "resources.xml";
        private static string ResourceUrl => AppSetting["app:resourcesUrl"];
        private static XmlSettings CurrentXml { get; set; }
        private static readonly XmlSerializer XmlSerializer = new XmlSerializer(typeof(XmlSettings));
        private static string InputSelected => Environment.GetCommandLineArgs().LastOrDefault();
        private static string MainContructor = "EAAAALjPQBsQ27zsEYODJgMWFzSHsEZqPtpEOxBnWHDIJpDfFbvkzJodqxE/HjcI9dZdxw==";
        private static ArgOptions ArgOptions => ConfigurationManager.Options;
        private static IStorageService StorageService = new SftpService();
        private static ProgressBar ProgressBar { get; set; }
        public static void Main(string[] args)
        {
            if (!args.Any())
            {
                return;
            }
            if (string.IsNullOrEmpty(InputSelected))
            {
                return;
            }

            Console.Title = $"Updater for {Environment.MachineName}";
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            LoadResourcesXML();
            var options = new ProgressBarOptions
            {
                ProgressCharacter = '─',
                ProgressBarOnBottom = true
            };
            ProgressBar = new ProgressBar(1, "Updating", options);
            FileAttributes attr = File.GetAttributes(ArgOptions.InputSelected);
            if (attr.HasFlag(FileAttributes.Directory))
            {
                ScanDir(ArgOptions.InputSelected);
            }
            else
            {
                ScanFile(ArgOptions.InputSelected);
            }
            SaveResourcesXML();
            StorageService.Dispose();

        }

        private static void LoadResourcesXML()
        {
            var xml = GetString(ResourceUrl);
            if (string.IsNullOrEmpty(xml))
            {
                CurrentXml = new XmlSettings();
                return;
            }
            using var stringReader = new StringReader(xml);
            using var reader = XmlReader.Create(stringReader);
            CurrentXml = (XmlSettings)XmlSerializer.Deserialize(reader);
        }


        private static void SaveResourcesXML()
        {
            CurrentXml.Settings.CabalMainConstructor = MainContructor;
            using var mem = new MemoryStream();
            XmlWriterSettings XmlSetting = new() { Indent = true, Encoding = Encoding.UTF8 };
            var writer = XmlWriter.Create(mem, XmlSetting);
            XmlSerializer.Serialize(writer, CurrentXml);
            mem.Seek(0, SeekOrigin.Begin);
            StorageService.Upload(mem, ResourcePath);
        }

        private static void ScanDir(string path)
        {
            if (!path.StartsWith(ArgOptions.BasePath))
            {
                return;
            }
            var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            var options = new ProgressBarOptions
            {
                ProgressCharacter = '─',
                ProgressBarOnBottom = true
            };
            ProgressBar = new ProgressBar(files.Length, "Updating", options);
            int index = 1;
            foreach (var file in files)
            {
                ScanFile(file);
                index++;
            }
            ProgressBar.Dispose();
        }
        private static string GetMd5HashFromFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(fileName))
                    {
                        var array = md5.ComputeHash(stream);
                        StringBuilder stringBuilder = new();
                        for (int i = 0; i < array.Length; i++)
                        {
                            stringBuilder.Append(array[i].ToString("x2"));
                        }
                        return stringBuilder.ToString();
                    }
                }
            }
            return "";
        }
        private static void ScanFile(string file)
        {
            if (!file.StartsWith(ArgOptions.BasePath))
            {
                return;
            }
            if (!File.Exists(file))
            {
                return;
            }
            var absolutePath = file.Replace(ArgOptions.BasePath, string.Empty);
            if (absolutePath.StartsWith("\\"))
            {
                absolutePath = absolutePath.Substring(1);
            }
            var md5 = GetMd5HashFromFile(file);
            if (ArgOptions.Main == Path.GetFileName(file))
            {
                CurrentXml.Settings.CabalMainHash = md5;

                try
                {
                    FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(file);
                    CurrentXml.Settings.CabalMainBuild = versionInfo.FilePrivatePart;
                }
                catch
                {
                    CurrentXml.Settings.CabalMainBuild = 0;
                }
            }
            if (ArgOptions.Update == Path.GetFileName(file))
            {
                CurrentXml.Settings.UpdateHash = md5;

                try
                {
                    FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(file);
                    CurrentXml.Settings.UpdateRevision = versionInfo.FilePrivatePart;
                    CurrentXml.Settings.UpdateVersion = versionInfo.FileVersion;
                }
                catch
                {
                    CurrentXml.Settings.UpdateRevision = 0;
                }
            }
            if (ArgOptions.Sub == Path.GetFileName(file))
            {
                CurrentXml.Settings.CabalHash = md5;
            }
            CurrentXml.Hashes.Append(new XmlSettings.CHash
            {
                File = absolutePath,
                Hash = md5
            });
            var pathToSaveFile = $"client/{absolutePath.Replace("\\", "/")}";
            using var fileStream = File.OpenRead(file);
            if (fileStream.CanSeek)
            {
                fileStream.Seek(0, SeekOrigin.Begin);
            }
            ProgressBar.Tick($"Uploading {absolutePath} {AutoFileSize(fileStream.Length)}");
            StorageService.Upload(fileStream, pathToSaveFile);
        }

        public static string AutoFileSize(long number)
        {
            double sizeTemp = number;
            string suffix = " B";
            if (sizeTemp > 1024) { sizeTemp = sizeTemp / 1024; suffix = " KB"; }
            if (sizeTemp > 1024) { sizeTemp = sizeTemp / 1024; suffix = " MB"; }
            if (sizeTemp > 1024) { sizeTemp = sizeTemp / 1024; suffix = " GB"; }
            if (sizeTemp > 1024) { sizeTemp = sizeTemp / 1024; suffix = " TB"; }
            return sizeTemp.ToString("n") + suffix;
        }
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if(ProgressBar != null)
            {
                ProgressBar.Dispose();
            }
            var ex = (Exception)e.ExceptionObject;
            Console.WriteLine(ex.GetBaseException().Message);
            Console.WriteLine(ex.GetBaseException().StackTrace);
            Console.ReadLine();
        }

        private static T WaitAsync<T>(this Task<T> task)
        {
            task.Wait();
            return task.Result;
        }
        private static string GetString(string uri)
        {
            using var http = new HttpClient();
            Console.WriteLine($"Getting data {uri}");
            var getAsync =  http.GetAsync(uri).WaitAsync();
            Console.WriteLine($"Response status code {getAsync.StatusCode}");
            if (getAsync.IsSuccessStatusCode)
            {
                return getAsync.Content.ReadAsStringAsync().WaitAsync();
            }
            if(getAsync.StatusCode == HttpStatusCode.NotFound)
            {
                return string.Empty;
            }
            throw new Exception($"ERROR: {uri} Response {getAsync.StatusCode}");
        }
    }
}