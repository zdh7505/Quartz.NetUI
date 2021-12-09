using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Quartz.NET.Web
{
    public class Program
    {

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args).UseKestrel().UseUrls("http://*:9950")
        //        .UseStartup<Startup>();

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
               Host.CreateDefaultBuilder(args)
                   .ConfigureWebHostDefaults(webBuilder =>
                   {
                       webBuilder.UseKestrel().UseUrls("http://*:9950");
                       webBuilder.UseIIS();
                       webBuilder.UseStartup<Startup>();
                   })
                   .ConfigureLogging(logging =>
                   {
                       logging.ClearProviders(); // 这个方法会清空所有控制台的输出
                       logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Error);
                   })
                   .UseNLog(); // 使用NLog
    }
}
