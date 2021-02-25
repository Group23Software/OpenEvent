using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Models.Category;

namespace OpenEvent.Web
{
    public static class SeedDataExtension
    {
        public static IWebHost SeedData(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetService<ApplicationContext>();

                context.Database.EnsureCreated();

                using (context)
                {
                    context.Categories.Add(new Category {Name = "Music"});
                    context.SaveChanges();
                }
            }

            return host;
        }
    }
}