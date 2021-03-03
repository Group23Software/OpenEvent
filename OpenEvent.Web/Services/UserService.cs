using System;
using System.Collections.Generic;
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
using OpenEvent.Web.Models.Analytic;
using OpenEvent.Web.Models.BankAccount;
using OpenEvent.Web.Models.PaymentMethod;
using OpenEvent.Web.Models.Recommendation;
using OpenEvent.Web.Models.User;
using Stripe;
using Address = OpenEvent.Web.Models.Address.Address;

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

        Task<UsersAnalytics> GetUsersAnalytics(Guid id);

        // Task<User> UpdateBasicInfo(UserAccountModel user);
        Task<string> UpdateAvatar(Guid id, byte[] avatar);
        Task<string> UpdateUserName(Guid id, string name);
        Task<bool> UserNameExists(string username);
        Task<bool> EmailExists(string email);
        Task<bool> PhoneExists(string phoneNumber);
        Task<bool> UpdateThemePreference(Guid id, bool isDarkMode);
        Task<bool> HostOwnsEvent(Guid eventId, Guid userId);

        Task<Address> UpdateAddress(Guid id, Address address);
    }

    /// <summary>
    /// Service providing all logic for user manipulation and review
    /// </summary>
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> Logger;
        private readonly ApplicationContext ApplicationContext;
        private readonly IMapper Mapper;
        private readonly IAuthService AuthService;

        /// <summary>
        /// UserService default constructor
        /// </summary>
        /// <param name="context"><see cref="ApplicationContext"/>></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="authService"><see cref="IAuthService"/>></param>
        public UserService(ApplicationContext context, ILogger<UserService> logger, IMapper mapper,
            IAuthService authService, IOptions<AppSettings> appSettings)
        {
            Logger = logger;
            ApplicationContext = context;
            Mapper = mapper;
            AuthService = authService;
            StripeConfiguration.ApiKey = appSettings.Value.StripeApiKey;
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
                x.Email == userInput.Email || x.UserName == userInput.UserName ||
                x.PhoneNumber == userInput.PhoneNumber);

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

            var categories = await ApplicationContext.Categories.ToListAsync();
            newUser.RecommendationScores =
                categories.Select(x => new RecommendationScore() {Category = x, Weight = 0}).ToList();

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
            var userRaw = ApplicationContext.Users.Include(x => x.BankAccounts).Include(x => x.PaymentMethods).AsSplitQuery().AsNoTracking().FirstOrDefault(x => x.Id == id);

            if (userRaw == null)
            {
                Logger.LogInformation("User not found");
                throw new UserNotFoundException();
            }

            var user = new UserAccountModel
            {
                Id = userRaw.Id,
                Avatar = Encoding.UTF8.GetString(userRaw.Avatar, 0, userRaw.Avatar.Length),
                Email = userRaw.Email,
                FirstName = userRaw.FirstName,
                LastName = userRaw.LastName,
                PhoneNumber = userRaw.PhoneNumber,
                UserName = userRaw.UserName,
                DateOfBirth = userRaw.DateOfBirth,
                IsDarkMode = userRaw.IsDarkMode,
                Address = userRaw.Address,
                StripeAccountId = userRaw.StripeAccountId,
                StripeCustomerId = userRaw.StripeCustomerId,
                PaymentMethods = userRaw.PaymentMethods != null
                    ? userRaw.PaymentMethods.Select(p => Mapper.Map<PaymentMethodViewModel>(p)).ToList()
                    : new List<PaymentMethodViewModel>(),
                BankAccounts = userRaw.BankAccounts != null
                    ? userRaw.BankAccounts.Select(p => Mapper.Map<BankAccountViewModel>(p)).ToList()
                    : new List<BankAccountViewModel>(),
            };

            if (user.StripeAccountId != null)
            {
                var service = new AccountService();
                var stripeAccount = service.Get(user.StripeAccountId);

                if (stripeAccount == null)
                {
                    Logger.LogInformation("Stripe user not found");
                    throw new UserNotFoundException();
                }
                
                user.StripeAccountInfo = Mapper.Map<StripeAccountInfo>(stripeAccount);
            }

            return user;
        }

        public async Task<UsersAnalytics> GetUsersAnalytics(Guid id)
        {
            var pageViewEvents = await ApplicationContext.PageViewEvents.Include(x => x.Event).AsSplitQuery().AsNoTracking().Where(x => x.User.Id == id).ToListAsync();
            var searchEvents = await ApplicationContext.SearchEvents.Where(x => x.User.Id == id).AsNoTracking().ToListAsync();
            var recommendationScores = await ApplicationContext.RecommendationScores.Include(x => x.Category).Where(x => x.User.Id == id)
                .AsNoTracking().ToListAsync();
            var ticketVerificationEvents = await ApplicationContext.VerificationEvents.Include(x => x.Ticket).Include(x => x.Event).Where(x => x.User.Id == id)
                .AsNoTracking().ToListAsync();

            return new UsersAnalytics
            {
                SearchEvents = searchEvents.Select(x => Mapper.Map<SearchEventViewModel>(x)).OrderByDescending(x => x.Created).ToList(),
                PageViewEvents = pageViewEvents.Select(x => Mapper.Map<PageViewEventViewModel>(x)).OrderByDescending(x => x.Created).ToList(),
                RecommendationScores = recommendationScores.Select(x => Mapper.Map<RecommendationScoreViewModel>(x)).ToList(),
                TicketVerificationEvents = ticketVerificationEvents.Select(x => Mapper.Map<TicketVerificationEventViewModel>(x)).OrderByDescending(x => x.Created).ToList()
            };
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

        /// <summary>
        /// Updates users theme preference.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isDarkMode"></param>
        /// <returns></returns>
        /// <exception cref="UserNotFoundException"></exception>
        public async Task<bool> UpdateThemePreference(Guid id, bool isDarkMode)
        {
            var user = await User(id);

            if (user == null)
            {
                Logger.LogInformation("User not found");
                throw new UserNotFoundException();
            }

            user.IsDarkMode = isDarkMode;

            try
            {
                await ApplicationContext.SaveChangesAsync();
                Logger.LogInformation("User's theme preference updated");
                return user.IsDarkMode;
            }
            catch
            {
                Logger.LogWarning("User failed to save");
                throw;
            }
        }

        /// <summary>
        /// Determines if the user owns the event supplied.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> HostOwnsEvent(Guid eventId, Guid userId)
        {
            var e = (await ApplicationContext.Events.Include(x => x.Host)
                .FirstOrDefaultAsync(x => x.Id == eventId && x.Host.Id == userId)) != null;
            Logger.LogInformation("Checking if user owns event", e);
            return e;
        }

        /// <summary>
        /// Updates users address.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        /// <exception cref="UserNotFoundException"></exception>
        public async Task<Address> UpdateAddress(Guid id, Address address)
        {
            var user = await User(id);

            if (user == null)
            {
                Logger.LogInformation("User not found");
                throw new UserNotFoundException();
            }

            user.Address = address;

            try
            {
                await ApplicationContext.SaveChangesAsync();
                Logger.LogInformation("User's address updated");
                return address;
            }
            catch
            {
                Logger.LogWarning("User failed to save");
                throw;
            }
        }
    }
}