using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpenEvent.Test.Seeds;
using OpenEvent.Web.Contexts;

namespace OpenEvent.Test.Setups
{
    public class BasicSetup
    {
        public async Task<ApplicationContext> Setup()
        {
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "openEvent").Options;

            await using var context = new ApplicationContext(dbContextOptions);
            Console.WriteLine("Starting data seed");
            await BasicSeed.SeedAsync(context);
            return new ApplicationContext(dbContextOptions);
        }
    }
}