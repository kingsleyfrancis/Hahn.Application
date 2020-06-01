using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;

namespace Hahn.ApplicatonProcess.May2020.Web
{
    public class Program
    {
       

        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
           .AddEnvironmentVariables()
           .Build();

        public static int Main(string[] args)
        {
            var logname = Configuration.GetValue<string>("LogName");
            Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(Configuration.GetSection("Serilog"))
            .Enrich.FromLogContext()
           //.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
           //.WriteTo.File()
           .CreateLogger();


            try{
                Log.Information("Starting up the application...");

                CreateHostBuilder(args).Build().Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {

            return Host.CreateDefaultBuilder(args)
                        .ConfigureLogging(logging =>
                           logging
                              .AddFilter("Microsoft", LogLevel.Information)
                                .AddFilter("System", LogLevel.Error))
                            .UseSerilog()                        
                            .ConfigureWebHostDefaults(webBuilder =>
                            {
                                webBuilder.UseStartup<Startup>();
                            });
        }
            
    }
}
