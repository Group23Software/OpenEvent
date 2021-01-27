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
using OpenEvent.Web.Models;

namespace OpenEvent.Web.Services
{
    public interface IAuthService
    {
        Task<UserViewModel> Authenticate(string email, string password, bool remember);
        Task<UserViewModel> Authenticate(Guid id);
        Task ForgotPassword(string email);
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
                Email = user.Email,
                Avatar = user.Avatar,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
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
                Logger.LogInformation("User does not exist");
                throw new Exception("User does not exist");
            }

            return Mapper.Map<UserViewModel>(user);
            
            //TODO: Authenticate with existing token saved as cookie
            throw new NotImplementedException();
        }

        public Task ForgotPassword(string email)
        {
            throw new System.NotImplementedException();
        }
    }
}