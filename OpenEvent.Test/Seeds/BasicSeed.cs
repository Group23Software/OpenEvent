using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Models;

namespace OpenEvent.Test.Seeds
{
    public class BasicSeed
    {
        public static async Task SeedAsync(ApplicationContext context)
        {
            PasswordHasher<User> hasher = new PasswordHasher<User>(
                new OptionsWrapper<PasswordHasherOptions>(
                    new PasswordHasherOptions()
                    {
                        CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2
                    })
            );
            
            User user = new User()
            {
                Id = new Guid("046E876E-D413-45AF-AC2A-552D7AA46C5C"),
                Email = "exists@email.co.uk",
                UserName = "ExistingUser",
                Password = "Password",
                FirstName = "exists",
                LastName = "already",
                PhoneNumber = "0000000000",
                Avatar = new Byte[]{1,1,1,1}
            };

            user.Password = hasher.HashPassword(user, "Password");

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }
    }
}