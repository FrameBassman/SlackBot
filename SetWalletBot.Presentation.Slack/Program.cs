using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SetWalletBot.Presentation.Slack
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseIISIntegration()
                .UseKestrel()
                //.UseContentRoot("")
                .UseStartup<Startup>()
                .Build();
    }
}