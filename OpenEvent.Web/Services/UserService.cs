using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Models;

namespace OpenEvent.Web.Services
{
    public interface IUserService
    {
        Task<User> Create(NewUserInput userInput);
        Task Destroy(Guid id);
    }

    public class UserService : IUserService
    {
        private ILogger<UserService> Logger;
        private ApplicationContext ApplicationContext;

        public UserService(ApplicationContext context, ILogger<UserService> logger)
        {
            Logger = logger;
            ApplicationContext = context;
        }
        
        public async Task<User> Create(NewUserInput userInput)
        {
            var user = await ApplicationContext.Users.FirstOrDefaultAsync(x => x.Email == userInput.Email || x.UserName == userInput.UserName);

            if (user != null)
            {
                Logger.LogInformation("User already exists");
                // TODO: custom or more appropriate exception
                throw new Exception("User already exists");
            }
            
            PasswordHasher<User> hasher = new PasswordHasher<User>(
                new OptionsWrapper<PasswordHasherOptions>(
                    new PasswordHasherOptions()
                    {
                        CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2
                    })
            );
            
            User newUser = new User
            {
                Email = userInput.Email,
                Avatar = userInput.Avatar,
                FirstName = userInput.FirstName,
                LastName = userInput.LastName,
                PhoneNumber = userInput.PhoneNumber,
                UserName = userInput.UserName
            };

            newUser.Password = hasher.HashPassword(newUser, userInput.Password);

            try
            {
                await ApplicationContext.Users.AddAsync(newUser);
                await ApplicationContext.SaveChangesAsync();
                newUser.Password = null;
                return newUser;
            }
            catch
            {
                Logger.LogWarning("User failed to save");
                throw;
            }

        }

        public async Task Destroy(Guid id)
        {
            var user = await ApplicationContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                Logger.LogInformation("User doesnt exist");
                throw new Exception("User doesnt exist");
            }

            try
            {
                ApplicationContext.Users.Remove(user);
                await ApplicationContext.SaveChangesAsync();
            }
            catch
            {
                Logger.LogInformation("User failed to save");
                throw;
            }
            
        }
    }
}