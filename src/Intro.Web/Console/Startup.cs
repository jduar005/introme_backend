using Intro.Web.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Nancy.Owin;

namespace NancyApplication
{
    public class Startup
    {
        private readonly IConfiguration config;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                              .AddJsonFile("appsettings.json")
                              .SetBasePath(env.ContentRootPath);

            config = builder.Build();
        }

        public void Configure(IApplicationBuilder app)
        {
            var appConfig = new AppConfiguration();
            config.Bind(appConfig);

            app.UseOwin(x => x.UseNancy());
        }
    }
}