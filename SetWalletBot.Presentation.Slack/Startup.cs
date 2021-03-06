﻿using Common.Logging;
using Common.Logging.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SetWalletBot.Presentation.Slack.Configuration;

namespace SetWalletBot.Presentation.Slack
{
    public class Startup
    {
        public static IConfigurationRoot Configuration { get; set; }
        
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            
            
            var logConfiguration = new LogConfiguration();
            Configuration.GetSection("LogConfiguration").Bind(logConfiguration);
            LogManager.Configure(logConfiguration);
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Adds services required for using options.
            //services.AddOptions();

            // Register the IConfiguration instance which MyOptions binds against.
            //services.Configure<MyOptions>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            
            var noobHost = new NoobotHost(new DotnetCoreConfigReader(Configuration.GetSection("Bot")));
            applicationLifetime.ApplicationStarted.Register(() => noobHost.Start(loggerFactory.CreateLogger("NoobHostLog")));
            applicationLifetime.ApplicationStopping.Register(noobHost.Stop);
        }
    }
}