using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SettingsSampleApp
{
    public class Startup
    {
        private readonly Config _config;
        
        public Startup(IConfiguration cfg)
        {
            _config = new();
            cfg.Bind(_config);

        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }
        
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync($"Config values are {nameof(_config.ValueA)} : {_config.ValueA}," +
                                                      $" {nameof(_config.ValueB)} : {_config.ValueB}, " +
                                                      $"{nameof(_config.ValueC)} : {_config.ValueC} ");
                });
            });
        }
    }

    public class Config
    {
        public string ValueA { get; set; }
        public string ValueB { get; set; }
        public string ValueC { get; set; }
    }
}