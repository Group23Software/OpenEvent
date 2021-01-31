using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.User;

namespace OpenEvent.Web.Services
{
    /// <summary>
    /// UserService interface
    /// </summary>
    public interface IUserService
    {
        Task<UserViewModel> Create(NewUserInput userInput);
        Task Destroy(Guid id);
        Task<UserAccountModel> Get(Guid id);
        // Task<User> UpdateBasicInfo(UserAccountModel user);
        Task<string> UpdateAvatar(Guid id, byte[] avatar);
        Task<string> UpdateUserName(Guid id, string name);
        Task<bool> UserNameExists(string username);
        Task<bool> EmailExists(string email);
        Task<bool> PhoneExists(string phoneNumber);
    }

    /// <summary>
    /// Service providing all logic for user manipulation and review
    /// </summary>
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> Logger;
        private readonly  ApplicationContext ApplicationContext;
        private readonly  IMapper Mapper;
        private readonly  IAuthService AuthService;

        /// <summary>
        /// UserService default constructor
        /// </summary>
        /// <param name="context"><see cref="ApplicationContext"/>></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="authService"><see cref="IAuthService"/>></param>
        public UserService(ApplicationContext context, ILogger<UserService> logger, IMapper mapper,
            IAuthService authService)
        {
            Logger = logger;
            ApplicationContext = context;
            Mapper = mapper;
            AuthService = authService;
        }

        /// <summary>
        /// Method for creating a new user.
        /// </summary>
        /// <param name="userInput">All user data collected for signup <see cref="NewUserInput"/>.</param>
        /// <returns>
        /// A task of type <see cref="UserViewModel"/> representing basic user information.
        /// </returns>
        /// <exception cref="UserAlreadyExistsException">Thrown when there is already a user with the same email, phone number or username.</exception>
        public async Task<UserViewModel> Create(NewUserInput userInput)
        {
            var user = await ApplicationContext.Users.FirstOrDefaultAsync(x =>
                x.Email == userInput.Email || x.UserName == userInput.UserName || x.PhoneNumber == userInput.PhoneNumber);

            if (user != null)
            {
                Logger.LogInformation("User already exists");
                throw new UserAlreadyExistsException();
            }

            // New password hasher.
            PasswordHasher<User> hasher = new PasswordHasher<User>(
                new OptionsWrapper<PasswordHasherOptions>(
                    new PasswordHasherOptions()
                    {
                        CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2
                    })
            );

            // Creating user object from params.
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

            // Hash user's password.
            newUser.Password = hasher.HashPassword(newUser, userInput.Password);

            try
            {
                // Saving user to Db.
                await ApplicationContext.Users.AddAsync(newUser);
                await ApplicationContext.SaveChangesAsync();
                return await AuthService.Login(newUser.Email, userInput.Password, userInput.Remember);
            }
            catch
            {
                Logger.LogWarning("User failed to save");
                throw;
            }
        }

        /// <summary>
        /// Method for removing user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// A completed Task once deleted.
        /// </returns>
        /// <exception cref="UserNotFoundException">Thrown when user can't be found.</exception>
        public async Task Destroy(Guid id)
        {
            var user = await User(id);

            if (user == null)
            {
                Logger.LogInformation("User doesnt exist");
                throw new UserNotFoundException();
            }

            try
            {
                // Remove user from Db.
                ApplicationContext.Users.Remove(user);
                await ApplicationContext.SaveChangesAsync();
                Logger.LogInformation("Destroyed user");
                
            }
            catch
            {
                Logger.LogInformation("User failed to save");
                throw;
            }
        }

        /// <summary>
        /// Method for getting user data for the account page.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Task of type <see cref="UserAccountModel"/> representing all data needed for account page.
        /// </returns>
        /// <exception cref="UserNotFoundException">Thrown when user can't be found.</exception>
        public async Task<UserAccountModel> Get(Guid id)
        {
            var user = await ApplicationContext.Users.Select(x => new UserAccountModel
            {
                Id = x.Id,
                Avatar = Encoding.UTF8.GetString(x.Avatar, 0, x.Avatar.Length),
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                PhoneNumber = x.PhoneNumber,
                UserName = x.UserName,
                DateOfBirth = x.DateOfBirth
            }).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                Logger.LogInformation("User not found");
                throw new UserNotFoundException();
            }

            return user;
        }

        /// <summary>
        /// Method for updating the users avatar.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="avatar">byte array of bitmap image</param>
        /// <returns>
        /// Task of string encoded bitmap.
        /// </returns>
        /// <exception cref="UserNotFoundException">Thrown when user can't be found.</exception>
        public async Task<string> UpdateAvatar(Guid id, byte[] avatar)
        {
            var user = await User(id);
            
            if (user == null)
            {
                Logger.LogInformation("User not found");
                throw new UserNotFoundException();
            }

            user.Avatar = avatar;

            try
            {
                await ApplicationContext.SaveChangesAsync();
                Logger.LogInformation("User's avatar updated");
                return Encoding.UTF8.GetString(user.Avatar, 0, user.Avatar.Length);
            }
            catch
            {
                Logger.LogWarning("User failed to save");
                throw;
            }
        }

        /// <summary>
        /// Method for updating the users username.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns>
        /// Task of username string.
        /// </returns>
        /// <exception cref="UserNotFoundException">Thrown when user can't be found.</exception>
        /// <exception cref="UserNameAlreadyExistsException">Thrown when a user with that username already exists.</exception>
        public async Task<string> UpdateUserName(Guid id, string name)
        {
            var user = await User(id);
            
            if (user == null)
            {
                Logger.LogInformation("User not found");
                throw new UserNotFoundException();
            }

            var exists = await UserNameExists(name);

            if (exists)
            {
                Logger.LogInformation("Username already exists");
                throw new UserNameAlreadyExistsException();
            }

            user.UserName = name;
            
            try
            {
                await ApplicationContext.SaveChangesAsync();
                Logger.LogInformation("User's username updated");
                return user.UserName;
            }
            catch
            {
                Logger.LogWarning("User failed to save");
                throw;
            }
        }

        private async Task<User> User(Guid id)
        {
            return await ApplicationContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Method for checking if a user with a username exists.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>
        /// Task of bool if username exists.
        /// </returns>
        public async Task<bool> UserNameExists(string username)
        {
            return await ApplicationContext.Users.FirstOrDefaultAsync(x => x.UserName == username) != null;
        }

        /// <summary>
        /// Method for checking if a user with a email exists.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>
        /// Task of bool if email exists.
        /// </returns>
        public async Task<bool> EmailExists(string email)
        {
            return await ApplicationContext.Users.FirstOrDefaultAsync(x => x.Email == email) != null;
        }
        
        /// <summary>
        /// Method for checking if a user with a phone number exists.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns>
        /// Task of bool if phone number exists.
        /// </returns>
        public async Task<bool> PhoneExists(string phoneNumber)
        {
            return await ApplicationContext.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber) != null;
        }
    }
}