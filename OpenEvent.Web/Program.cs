using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Models.Category;

namespace OpenEvent.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
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

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}