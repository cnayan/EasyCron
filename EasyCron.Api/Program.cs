
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

using System.IO;

namespace Bayantu.Evos.Services.CronJob.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static IWebHost BuildWebHost(string[] args)
        {
            // hosting.json的配置应该比之后的appsettings配置要早，所以这个时候是没有办法用环境变量来决定运行环境的
            var configCommandline = new ConfigurationBuilder()
                .AddJsonFile("hosting.json", false, true)
                .AddCommandLine(args).Build();

            return WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(configCommandline)
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.AddJsonFile($"appsettings.{builderContext.HostingEnvironment.EnvironmentName}.json", false, true);
                })
                .Build();
        }
    }
}
