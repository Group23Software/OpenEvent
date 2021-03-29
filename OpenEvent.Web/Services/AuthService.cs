using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenEvent.Data.Models.User;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Exceptions;

namespace OpenEvent.Web.Services
{
    /// <inheritdoc />
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> Logger;
        private readonly ApplicationContext ApplicationContext;
        private readonly AppSettings AppSettings;
        private readonly IMapper Mapper;
        private readonly IEmailService EmailService;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context"><see cref="ApplicationContext"/>></param>
        /// <param name="logger"></param>
        /// <param name="appSettings"></param>
        /// <param name="mapper"></param>
        /// <param name="emailService"></param>
        public AuthService(ApplicationContext context, ILogger<AuthService> logger, IOptions<AppSettings> appSettings, IMapper mapper, IEmailService emailService)
        {
            Logger = logger;
            ApplicationContext = context;
            AppSettings = appSettings.Value;
            Mapper = mapper;
            EmailService = emailService;
        }


        /// <inheritdoc />
        /// <exception cref="UserNotFoundException">Thrown when user can't be found.</exception>
        /// <exception cref="IncorrectPasswordException">Thrown when the password is incorrect.</exception>
        public async Task<UserViewModel> Login(string email, string password, bool remember)
        {
            var user = await ApplicationContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                Logger.LogInformation("User not found");
                throw new UserNotFoundException();
            }

            if (!user.Confirmed)
            {
                Logger.LogInformation("User not confirmed");
                throw new UserNotConfirmedException();
            }

            var hasher = PasswordHasher();

            // checks the stored hash with the input password hashed
            if (hasher.VerifyHashedPassword(user, user.Password, password) == PasswordVerificationResult.Failed)
            {
                Logger.LogInformation("Incorrect password");
                throw new IncorrectPasswordException();
            }

            UserViewModel userViewModel = new UserViewModel
            {
                Id = user.Id,
                Avatar = Encoding.UTF8.GetString(user.Avatar, 0, user.Avatar.Length),
                UserName = user.UserName,
                Token = GenerateToken(user, remember ? 30 : 1),
                IsDarkMode = user.IsDarkMode
            };

            return userViewModel;
        }

        // Generates a JWT token for the user for a number of days
        private string GenerateToken(User user, int days)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(days),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <inheritdoc />
        /// <exception cref="UserNotFoundException">Thrown when user can't be found.</exception>
        public async Task<UserViewModel> Authenticate(Guid id)
        {
            var user = await ApplicationContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                Logger.LogInformation("User not found");
                throw new UserNotFoundException();
            }
            
            if (!user.Confirmed)
            {
                Logger.LogInformation("User not confirmed");
                throw new UserNotConfirmedException();
            }

            return Mapper.Map<UserViewModel>(user);
        }
        
        /// <inheritdoc />
        /// <exception cref="UserNotFoundException">Thrown when the user is not found</exception>
        public async Task ForgotPassword(string email)
        {
            var user = await ApplicationContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                Logger.LogInformation("User not found");
                throw new UserNotFoundException();
            }

            try
            {
                await EmailService.SendAsync(user.Email,
                    $"<h1>Reset your password</h1><a href='{AppSettings.BaseUrl}/forgot/{user.Id}'>Reset Password</a>", "Forgot Password");
            }
            catch
            {
                Logger.LogWarning("Failed to email user");
                throw;
            }
        }

        /// <inheritdoc />
        /// <exception cref="UserNotFoundException">Thrown when user can't be found.</exception>
        public async Task UpdatePassword(Guid id, string password)
        {
            var user = await ApplicationContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                Logger.LogInformation("User not found");
                throw new UserNotFoundException();
            }

            var hasher = PasswordHasher();

            user.Password = hasher.HashPassword(user, password);

            try
            {
                await ApplicationContext.SaveChangesAsync();
            }
            catch
            {
                Logger.LogWarning("User failed to save");
                throw;
            }
        }

        /// <inheritdoc />
        /// <exception cref="UserNotFoundException"></exception>
        public async Task ConfirmEmail(Guid id)
        {
            var user = await ApplicationContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                Logger.LogInformation("User not found");
                throw new UserNotFoundException();
            }

            user.Confirmed = true;
            
            try
            {
                await ApplicationContext.SaveChangesAsync();
            }
            catch
            {
                Logger.LogWarning("User failed to save");
                throw;
            }
        }

        // Returns a password hasher for hashing and comparing passwords
        private static PasswordHasher<User> PasswordHasher()
        {
            PasswordHasher<User> hasher = new PasswordHasher<User>(
                new OptionsWrapper<PasswordHasherOptions>(
                    new PasswordHasherOptions()
                    {
                        CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2
                    })
            );
            return hasher;
        }
    }
}