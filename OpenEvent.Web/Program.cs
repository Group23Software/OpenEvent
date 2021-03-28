using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Models.Category;
using Prometheus;
using Serilog;

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
        public static void Main(string[] args)
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

                context.Database.EnsureCreated();

                using (context)
                {
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
                }
            }

            host.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.Seq("http://localhost:80", apiKey: "UgrmhkMuEVCZxOX89WUm")
                )
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}