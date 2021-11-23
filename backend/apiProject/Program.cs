using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
/*                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.Limits.MaxRequestBodySize = 100 * 1024 * 1024;
                        serverOptions.Limits.MAx
                    });*/
                })
            .ConfigureAppConfiguration((hostingContext,builder) =>
            {
/*                var envName = hostingContext.HostingEnvironment.EnvironmentName.ToString().ToLower();
                builder.AddSystemsManager($"/{envName}", TimeSpan.FromMinutes(5));*/
                builder.AddJsonFile("appsettings.json");
                builder.AddSystemsManager("/comp306", new AWSOptions
                {
                    Region = RegionEndpoint.USEast1

                }, true, TimeSpan.FromMinutes(5));
            });
    }
}
