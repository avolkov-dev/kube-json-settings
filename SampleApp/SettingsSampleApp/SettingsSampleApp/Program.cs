using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using StackExchange.Utils;

namespace SettingsSampleApp
{
    class Program
    {
        static void Main(string[] args)
        {            
            var host = new WebHostBuilder()
                .UseKestrel()
                .ConfigureAppConfiguration(cfg =>
                {
                    var configLocator = SettingsLocator.Read();
                    cfg.WithPrefix(
                            "secrets",
                            c =>
                            {
                                foreach (var path in configLocator.SecretsFilePath)
                                {
                                    c.AddJsonFile(path, optional: false, reloadOnChange: true);
                                }
                            }
                        )
                        .WithSubstitution(
                            c =>
                            {
                                foreach (var path in configLocator.SettingsFilePath)
                                {
                                    c.AddJsonFile(path, optional: false, reloadOnChange: true);
                                }
                            }
                        );
                    
                    // matches a key wrapped in braces and prefixed with a '$' 
                    // e.g. ${Key} or ${Section:Key} or ${Section:NestedSection:Key}
                    var substitutionPattern = new Regex(@"\$\{(?<key>[^\s]+?)\}", RegexOptions.Compiled);

                    foreach (var kv in cfg.Build().AsEnumerable())
                    {
                        if (kv.Value != null && substitutionPattern.Matches(kv.Value).Any())
                        {
                            throw new InvalidOperationException(
                                $"Configuration mismatch: secret value {kv.Value} not substituted for {kv.Key}");
                        }
                    }
                }) 
                .UseStartup<Startup>()
                .UseUrls("http://*:5000/")
                .Build();
            host.Run();
        }
    }

    class SettingsLocator
    {
        private SettingsLocator()
        {
            SecretsFilePath = new string[0];
            SettingsFilePath = new string[0];
        }
        
        public static SettingsLocator Read()
        {
            var cb = new ConfigurationBuilder().AddJsonFile("settings/settingslocator.json", optional: true).AddEnvironmentVariables().Build();
            
            var result = new SettingsLocator();
            
            cb.Bind(result);

            if (result.SettingsFilePath.Length == 0)
            {
                throw new InvalidOperationException("Settings not configured");
            }

            return result;
        }
        
        public string[] SecretsFilePath { get; set; }
        public string[] SettingsFilePath { get; set; }
    }
}