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
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Models.User;

namespace OpenEvent.Web.Services
{
    public interface IAuthService
    {
        Task<UserViewModel> Authenticate(string email, string password, bool remember);
        Task<UserViewModel> Authenticate(Guid id);
        Task ForgotPassword(string email);
        Task UpdatePassword(string email, string password);
    }

    public class AuthService : IAuthService
    {
        private ILogger<AuthService> Logger;
        private ApplicationContext ApplicationContext;
        private AppSettings AppSettings;
        private readonly IMapper Mapper;

        public AuthService(ApplicationContext context, ILogger<AuthService> logger, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            Logger = logger;
            ApplicationContext = context;
            AppSettings = appSettings.Value;
            Mapper = mapper;
        }

        public async Task<UserViewModel> Authenticate(string email, string password, bool remember)
        {
            var user = await ApplicationContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                Logger.LogInformation("User not found");
                throw new Exception("User not found");
            }

            PasswordHasher<User> hasher = new PasswordHasher<User>(
                new OptionsWrapper<PasswordHasherOptions>(
                    new PasswordHasherOptions()
                    {
                        CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2
                    })
            );

            if (hasher.VerifyHashedPassword(user, user.Password, password) == PasswordVerificationResult.Failed)
            {
                Logger.LogInformation("Incorrect password");
                throw new Exception("Incorrect password");
            }

            int days = remember ? 30 : 1;
            
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
            
            UserViewModel userViewModel = new UserViewModel()
            {
                Id = user.Id,
                Avatar = Encoding.UTF8.GetString(user.Avatar, 0, user.Avatar.Length),
                UserName = user.UserName,
                Token = tokenHandler.WriteToken(token)
            };

            return userViewModel;
        }

        public async Task<UserViewModel> Authenticate(Guid id)
        {
            var user = await ApplicationContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                Logger.LogInformation("User not found");
                throw new Exception("User not found");
            }

            return Mapper.Map<UserViewModel>(user);
        }

        public Task ForgotPassword(string email)
        {
            throw new System.NotImplementedException();
        }

        public async Task UpdatePassword(string email, string password)
        {
            var user = await ApplicationContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                Logger.LogInformation("User not found");
                throw new Exception("User not found");
            }

            PasswordHasher<User> hasher = new PasswordHasher<User>(
                new OptionsWrapper<PasswordHasherOptions>(
                    new PasswordHasherOptions()
                    {
                        CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2
                    })
            );

            user.Password = hasher.HashPassword(user, password);
            ApplicationContext.Entry(user).State = EntityState.Modified;

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
    }
}