using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        Task<UserViewModel> Create(NewUserInput userInput);
        Task Destroy(Guid id);

        Task<UserAccountModel> Get(Guid id);

        // Task<User> UpdateBasicInfo(UserAccountModel user);
        Task<bool> UserNameExists(string username);
        Task<bool> EmailExists(string email);
        Task<bool> PhoneExists(string phoneNumber);
    }

    public class UserService : IUserService
    {
        private ILogger<UserService> Logger;
        private ApplicationContext ApplicationContext;
        private IMapper Mapper;
        private IAuthService AuthService;

        public UserService(ApplicationContext context, ILogger<UserService> logger, IMapper mapper,
            IAuthService authService)
        {
            Logger = logger;
            ApplicationContext = context;
            Mapper = mapper;
            AuthService = authService;
        }

        public async Task<UserViewModel> Create(NewUserInput userInput)
        {
            var user = await ApplicationContext.Users.FirstOrDefaultAsync(x =>
                x.Email == userInput.Email || x.UserName == userInput.UserName);

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
                UserName = userInput.UserName,
                DateOfBirth = userInput.DateOfBirth
            };

            newUser.Password = hasher.HashPassword(newUser, userInput.Password);

            try
            {
                await ApplicationContext.Users.AddAsync(newUser);
                await ApplicationContext.SaveChangesAsync();
                // newUser.Password = null;
                // return Mapper.Map<UserViewModel>(newUser);
                return await AuthService.Authenticate(newUser.Email, userInput.Password, userInput.Remember);
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

        public async Task<UserAccountModel> Get(Guid id)
        {
            var user = await ApplicationContext.Users.Select(x => new UserAccountModel
            {
                Id = x.Id,
                Avatar = x.Avatar,
                Email = x.Email,
                Token = x.Token,
                FirstName = x.FirstName,
                LastName = x.LastName,
                PhoneNumber = x.PhoneNumber,
                UserName = x.UserName
            }).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                Logger.LogInformation("User not found");
                throw new Exception("User not found");
            }

            return user;
        }

        public async Task<bool> UserNameExists(string username)
        {
            return await ApplicationContext.Users.FirstOrDefaultAsync(x => x.UserName == username) != null;
        }

        public async Task<bool> EmailExists(string email)
        {
            return await ApplicationContext.Users.FirstOrDefaultAsync(x => x.Email == email) != null;
        }
        
        public async Task<bool> PhoneExists(string phoneNumber)
        {
            return await ApplicationContext.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber) != null;
        }

        // public async Task<User> UpdateBasicInfo(UserAccountModel user)
        // {
        //     var userCheck = await ApplicationContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
        //     
        //     if (userCheck == null)
        //     {
        //         Logger.LogInformation("User doesnt exist");
        //         throw new Exception("User doesnt exist");
        //     }
        //     
        //     // ApplicationContext.Entry(user).State = EntityState.Modified;
        //
        //     try
        //     {
        //         await ApplicationContext.SaveChangesAsync();
        //         return user;
        //     }
        //     catch
        //     {
        //         Logger.LogInformation("User failed to save");  
        //         throw;
        //     }
        // }
    }
}