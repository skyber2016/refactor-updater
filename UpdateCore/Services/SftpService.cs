using Renci.SshNet;
using System.Diagnostics;
using Update.Core;

namespace UpdateCore.Services
{
    public class SftpService : IStorageService
    {
        private string Prefix => ConfigurationManager.Settings["sftp:prefix"];
        private string _host => ConfigurationManager.Settings["sftp:host"];
        private int _port => Convert.ToInt32(ConfigurationManager.Settings["sftp:port"]);
        private string _username => ConfigurationManager.Settings["sftp:username"];
        private string _password => ConfigurationManager.Settings["sftp:password"];

        private SftpClient _sshClient;

        public SftpService()
        {
            _sshClient = new SftpClient(_host, _port, _username, _password);
        }

        public Stream Download(string pathToFile)
        {
            if(!_sshClient.IsConnected)
            {
                _sshClient.Connect();
            }
            if (_sshClient.IsConnected)
            {
                var mem = new MemoryStream();
                _sshClient.DownloadFile($"{Prefix}/{pathToFile}", mem);
                return mem;
            }
            return null;
        }

        public void Upload(Stream data, string pathToSave)
        {
            try
            {
                if (!_sshClient.IsConnected)
                {
                    _sshClient.Connect();
                }
                if (_sshClient.IsConnected)
                {
                    var dir = Path.GetDirectoryName(pathToSave).Replace("\\", "/");
                    var splitPath = dir.Split('/');
                    var currentPath = string.Empty;
                    foreach (var path in splitPath)
                    {
                        if (string.IsNullOrEmpty(currentPath))
                        {
                            currentPath = path;
                        }
                        else
                        {
                            currentPath += "/" + path;
                        }
                        if (string.IsNullOrEmpty(currentPath))
                        {
                            continue;
                        }
                        if (!_sshClient.Exists(currentPath))
                        {
                            _sshClient.CreateDirectory(currentPath);
                        }
                    }
                    _sshClient.UploadFile(data, pathToSave);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void Dispose()
        {
            if (_sshClient.IsConnected)
            {
                _sshClient.Disconnect();
            }
        }
    }
}
