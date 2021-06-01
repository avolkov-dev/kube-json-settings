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
                                foreach (var path in configLocator.Secrets)
                                {
                                    c.AddJsonFile(path, optional: false, reloadOnChange: true);
                                }
                            }
                        )
                        .WithSubstitution(
                            c =>
                            {
                                foreach (var path in configLocator.Settings)
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
            Secrets = new string[0];
            Settings = new string[0];
        }
        
        public static SettingsLocator Read()
        {
            var cb = new ConfigurationBuilder().AddJsonFile("settings/settingslocator.json", optional: true).AddEnvironmentVariables().Build();
            
            var result = new SettingsLocator();
            
            cb.Bind(result);

            if (result.Settings.Length == 0)
            {
                throw new InvalidOperationException("Settings not configured");
            }

            if (result.Secrets.Length == 0)
            {
                throw new InvalidOperationException("Secrets not configured");
            }

            return result;
        }
        
        public string[] Secrets { get; set; }
        public string[] Settings { get; set; }
    }
}