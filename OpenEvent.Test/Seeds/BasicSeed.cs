using System;
using System.Threading.Tasks;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Models;

namespace OpenEvent.Test.Seeds
{
    public class BasicSeed
    {
        public static async Task SeedAsync(ApplicationContext context)
        {
            User user = new User()
            {
                Id = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"),
                Email = "exists@email.co.uk",
                Password = "Password",
                UserName = "ExistingUser",
                FirstName = "exists",
                LastName = "already",
                PhoneNumber = "0000000000"
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }
    }
}