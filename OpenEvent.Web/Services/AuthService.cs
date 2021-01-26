using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Models;

namespace OpenEvent.Web.Services
{
    public interface IAuthService
    {
        Task<string> Authenticate(string email, string password);
        Task<string> Authenticate(string token);
        Task ForgotPassword(string email);
    }

    public class AuthService : IAuthService
    {
        private ILogger<AuthService> Logger;
        private ApplicationContext ApplicationContext;
        private AppSettings AppSettings;

        public AuthService(ApplicationContext context, ILogger<AuthService> logger, IOptions<AppSettings> appSettings)
        {
            Logger = logger;
            ApplicationContext = context;
            AppSettings = appSettings.Value;
        }
        
        public async Task<string> Authenticate(string email, string password)
        {
            var user = await ApplicationContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                Logger.LogInformation("User does not exist");
                throw new Exception("User does not exist");
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
                Logger.LogInformation("Password incorrect");
                throw new Exception("Password incorrect");
            }
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = null;

            return user.Token;
        }

        public Task<string> Authenticate(string token)
        {
            //TODO: Authenticate with existing token saved as cookie
            throw new NotImplementedException();
        }

        public Task ForgotPassword(string email)
        {
            throw new System.NotImplementedException();
        }
    }
}