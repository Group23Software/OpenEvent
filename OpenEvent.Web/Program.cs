using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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
        private static readonly Counter TickTock =
            Metrics.CreateCounter("openEvent_ticks_total", "Just keeps on ticking");

        /// <summary>
        /// Main method run on startup
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            // var server = new MetricServer(hostname: "localhost", port: 1234);
            // server.Start();
            //
            // while (true)
            // {
            //     TickTock.Inc();
            //     Thread.Sleep(TimeSpan.FromSeconds(1));
            // }

            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetService<ApplicationContext>();

                // context.Database.EnsureCreated();

                using (context)
                {
                    // await context.Database.EnsureDeletedAsync();
                    // await context.Database.EnsureCreatedAsync();
                    
                    if (!context.Categories.Any())
                    {
                        List<Category> categories = new List<Category>
                        {
                            new() {Name = "Music"},
                            new() {Name = "Business"},
                            new() {Name = "Charity"},
                            new() {Name = "Culture"},
                            new() {Name = "Family"},
                            new() {Name = "Education"},
                            new() {Name = "Fashion"},
                            new() {Name = "Film"},
                            new() {Name = "Media"},
                            new() {Name = "Food"},
                            new() {Name = "Politics"},
                            new() {Name = "Health"},
                            new() {Name = "Hobbies"},
                            new() {Name = "Lifestyle"},
                            new() {Name = "Other"},
                            new() {Name = "Performing"},
                            new() {Name = "Visual Arts"},
                            new() {Name = "Religion"},
                            new() {Name = "Science"},
                            new() {Name = "Technology"},
                            new() {Name = "Seasonal"},
                            new() {Name = "Sport"},
                            new() {Name = "Outdoor"},
                            new() {Name = "Travel"},
                            new() {Name = "Automobile"}
                        };

                        context.Categories.AddRange(categories);
                        context.SaveChanges();
                    }

                    // await Flagship.SeedDatabase(context, services);
                }
            }

            host.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, services, configuration) => configuration
                    // .ReadFrom.Configuration(context.Configuration)
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft",LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime",LogEventLevel.Information)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .Enrich.WithExceptionDetails()
                    .Enrich.WithCorrelationId()
                    .WriteTo.Console(theme: AnsiConsoleTheme.Code,outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3} {SourceContext}] {Message:lj}{NewLine}{Exception}")
                    .WriteTo.Seq("http://localhost:80", apiKey: "UgrmhkMuEVCZxOX89WUm")
                )
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}