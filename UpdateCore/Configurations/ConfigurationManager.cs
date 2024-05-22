using Microsoft.Extensions.Configuration;
using System.Net;
using System.Security.Principal;

namespace Update.Core
{
    public static class ConfigurationManager
    {
        public static IConfiguration Settings { get; }
        public static ArgOptions Options { get; }
        private static string Username => WindowsIdentity.GetCurrent().Name.Replace("\\", "_");
        private static string SettingUrl = $"https://update.zeroonlinevn.com/updater/settings.{Username}.yaml";

        static ConfigurationManager()
        {
            using var client = new HttpClient();
            Console.WriteLine($"Load config from {SettingUrl}");
            try
            {
                var streamResponse = client.GetStreamAsync(SettingUrl);
                streamResponse.Wait();
                Settings = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddYamlStream(streamResponse.Result)
                        .Build();

                Options = new ArgOptions()
                {
                    BasePath = Settings["app:basePath"],
                    InputSelected = Environment.GetCommandLineArgs().LastOrDefault(),
                    Main = Settings["app:main"],
                    Sub = Settings["app:sub"],
                    Update = Settings["app:update"]
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetBaseException().Message);
                Console.ReadLine();
            }
            
        }
    }
}
