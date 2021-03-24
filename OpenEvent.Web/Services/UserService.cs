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
using OpenEvent.Web.Models.Address;
using OpenEvent.Web.Models.Analytic;
using OpenEvent.Web.Models.BankAccount;
using OpenEvent.Web.Models.PaymentMethod;
using OpenEvent.Web.Models.Recommendation;
using OpenEvent.Web.Models.Transaction;
using OpenEvent.Web.Models.User;

namespace OpenEvent.Web.Services
{
    /// <summary>
    /// Service providing all logic for user manipulation and review
    /// </summary>
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> Logger;
        private readonly ApplicationContext ApplicationContext;
        private readonly IMapper Mapper;
        private readonly IEmailService EmailService;
        private readonly AppSettings AppSettings;


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="appSettings"></param>
        /// <param name="emailService"></param>
        public UserService(
            ApplicationContext context, ILogger<UserService> logger, IMapper mapper,
            IOptions<AppSettings> appSettings, IEmailService emailService)
        {
            Logger = logger;
            ApplicationContext = context;
            Mapper = mapper;
            AppSettings = appSettings.Value;
            Stripe.StripeConfiguration.ApiKey = appSettings.Value.StripeApiKey;
            EmailService = emailService;
        }

        /// <inheritdoc />
        /// <exception cref="UserAlreadyExistsException">Thrown when there is already a user with the same email, phone number or username.</exception>
        public async Task Create(NewUserBody userBody)
        {
            var user = await ApplicationContext.Users.FirstOrDefaultAsync(x =>
                x.Email == userBody.Email || x.UserName == userBody.UserName ||
                x.PhoneNumber == userBody.PhoneNumber);

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
                Email = userBody.Email,
                Avatar = userBody.Avatar,
                FirstName = userBody.FirstName,
                LastName = userBody.LastName,
                PhoneNumber = userBody.PhoneNumber,
                UserName = userBody.UserName,
                DateOfBirth = userBody.DateOfBirth,
                Confirmed = false
            };

            // Hash user's password.
            newUser.Password = hasher.HashPassword(newUser, userBody.Password);

            // Generates a new recommendation score for each category with weight 0 
            var categories = await ApplicationContext.Categories.ToListAsync();
            newUser.RecommendationScores =
                categories.Select(x => new RecommendationScore {Category = x, Weight = 0}).ToList();

            try
            {
                // Saving user to Db.
                await ApplicationContext.Users.AddAsync(newUser);
                await ApplicationContext.SaveChangesAsync();

                // Sends confirmation Email
                await EmailService.SendAsync(newUser.Email,
                    $"<h1>Please confirm your email</h1><a href={AppSettings.BaseUrl}/api/auth/confirm?id={newUser.Id}>confirm</a>",
                    "Confirm Email");
            }
            catch
            {
                Logger.LogWarning("User failed to save");
                throw;
            }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        /// <exception cref="UserNotFoundException">Thrown when user can't be found.</exception>
        public async Task<UserAccountModel> Get(Guid id)
        {
            var userRaw = await ApplicationContext.Users
                .Include(x => x.BankAccounts)
                .Include(x => x.PaymentMethods)
                .Include(x => x.Transactions).ThenInclude(x => x.Event)
                .AsSplitQuery().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

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
                Transactions = userRaw.Transactions.Any()
                    ? userRaw.Transactions.Select(x => Mapper.Map<TransactionViewModel>(x)).ToList()
                    : new List<TransactionViewModel>()
            };

            // If the user has a stripe account
            if (user.StripeAccountId != null)
            {
                var service = new Stripe.AccountService();
                
                // Gets users stripe account 
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

        /// <inheritdoc />
        public async Task<UsersAnalytics> GetAnalytics(Guid id)
        {
            var pageViewEvents = await ApplicationContext.PageViewEvents.Include(x => x.Event).AsSplitQuery()
                .AsNoTracking().Where(x => x.User.Id == id).ToListAsync();
            
            var searchEvents = await ApplicationContext.SearchEvents.Where(x => x.User.Id == id).AsSplitQuery()
                .AsNoTracking().ToListAsync();
            
            var recommendationScores = await ApplicationContext.RecommendationScores.Include(x => x.Category)
                .AsSplitQuery().AsNoTracking().Where(x => x.User.Id == id)
                .AsNoTracking().ToListAsync();
            
            var ticketVerificationEvents = await ApplicationContext.VerificationEvents.Include(x => x.Ticket)
                .Include(x => x.Event).AsSplitQuery().AsNoTracking().Where(x => x.User.Id == id)
                .AsNoTracking().ToListAsync();
            

            // Maps all results into ordered lists
            return new UsersAnalytics
            {
                SearchEvents = searchEvents.Select(x => Mapper.Map<SearchEventViewModel>(x)).OrderByDescending(x => x.Created).ToList(),
                PageViewEvents = pageViewEvents.Select(x => Mapper.Map<PageViewEventViewModel>(x)).OrderByDescending(x => x.Created).ToList(),
                RecommendationScores = recommendationScores.Select(x => Mapper.Map<RecommendationScoreViewModel>(x)).ToList(),
                TicketVerificationEvents = ticketVerificationEvents.Select(x => Mapper.Map<TicketVerificationEventViewModel>(x)).OrderByDescending(x => x.Created).ToList()
            };
        }

        /// <inheritdoc />
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
                
                // converts byte array to string
                return Encoding.UTF8.GetString(user.Avatar, 0, user.Avatar.Length);
            }
            catch
            {
                Logger.LogWarning("User failed to save");
                throw;
            }
        }

        /// <inheritdoc />
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

        // Gets a user with id
        private async Task<User> User(Guid id)
        {
            return await ApplicationContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <inheritdoc />
        public async Task<bool> UserNameExists(string username)
        {
            return await ApplicationContext.Users.FirstOrDefaultAsync(x => x.UserName == username) != null;
        }

        /// <inheritdoc />
        public async Task<bool> EmailExists(string email)
        {
            return await ApplicationContext.Users.FirstOrDefaultAsync(x => x.Email == email) != null;
        }

        /// <inheritdoc />
        public async Task<bool> PhoneExists(string phoneNumber)
        {
            return await ApplicationContext.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber) != null;
        }

        /// <inheritdoc />
        /// <exception cref="UserNotFoundException">Thrown when the user is not found</exception>
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
        
        /// <inheritdoc />
        public async Task<bool> HostOwnsEvent(Guid eventId, Guid userId)
        {
            var e = (await ApplicationContext.Events.Include(x => x.Host)
                .FirstOrDefaultAsync(x => x.Id == eventId && x.Host.Id == userId)) != null;
            Logger.LogInformation("Checking if user owns event", e);
            return e;
        }

        /// <inheritdoc />
        /// <exception cref="UserNotFoundException">Thrown when the user is not found</exception>
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