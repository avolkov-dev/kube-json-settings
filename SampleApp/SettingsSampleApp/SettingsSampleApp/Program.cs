using System;
using System.Collections.Generic;
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
                    cfg.WithPrefix(
                            "secrets",
                            c => c.AddJsonFile("secrets/secrets.json", optional: false, reloadOnChange: true)
                        )
                        .WithSubstitution(
                            c => c.AddJsonFile("settings/appsettings.json", optional: false, reloadOnChange: true)
                        );
                }) 
                .UseStartup<Startup>()
                .UseUrls("http://*:5000/")
                .Build();
            host.Run();
        }
    }
}