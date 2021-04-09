using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenEvent.Data.Models.Category;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Services;
using Prometheus;
using Serilog;
using Serilog.Enrichers.AspNetCore;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.SystemConsole.Themes;

namespace OpenEvent.Web
{
    /// <summary>
    /// Main program entry point
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method run on startup
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetService<ApplicationContext>();

                await using (context)
                {
                    if (!await context.Database.CanConnectAsync())
                    {
                        Console.WriteLine("can't find database creating and seeding");
                        
                        await context.Database.EnsureDeletedAsync();
                        await context.Database.EnsureCreatedAsync();

                        await Flagship.SeedCategories(context);
                        await Flagship.SeedDatabase(context, services);
                    }
                    else
                    {
                        if (!context.Categories.Any())
                        {
                            Console.WriteLine("database is empty. seeding now");
                            
                            await context.Database.EnsureDeletedAsync();
                            await context.Database.EnsureCreatedAsync();

                            await Flagship.SeedCategories(context);
                            await Flagship.SeedDatabase(context, services);
                        }
                    }
                }
            }

            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, services, configuration) => configuration
                    // .ReadFrom.Configuration(context.Configuration)
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .Enrich.WithExceptionDetails()
                    .Enrich.WithCorrelationId()
                    .WriteTo.Console(theme: AnsiConsoleTheme.Code, outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3} {SourceContext}] {Message:lj}{NewLine}{Exception}")
                    .WriteTo.Seq("http://localhost:80", apiKey: "UgrmhkMuEVCZxOX89WUm")
                )
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}