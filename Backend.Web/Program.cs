using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Backend.Web
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webHost =>
                {
                    webHost
                        .UseSetting(WebHostDefaults.SuppressStatusMessagesKey, true.ToString(CultureInfo.InvariantCulture))
                        .UseStartup<Startup>();
                });
        }
    }
}