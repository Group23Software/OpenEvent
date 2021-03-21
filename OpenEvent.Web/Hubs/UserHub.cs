using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Services;

namespace OpenEvent.Web.Hubs
{
    public class UserHub : Hub
    {
        private readonly ILogger<UserHub> Logger;
        private readonly IUserService UserService;

        public UserHub(ILogger<UserHub> logger, IUserService userService)
        {
            Logger = logger;
            UserService = userService;
        }

        public override async Task OnConnectedAsync()
        {
            Logger.LogInformation("{Id} has connected to user hub",Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Logger.LogInformation("{Id} has disconnected from user hub",Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task<bool> UserNameExists(string username)
        { 
            return await UserService.UserNameExists(username);
        }
        
        public async Task<bool> EmailExists(string email)
        { 
            return await UserService.EmailExists(email);
        } 
        
        public async Task<bool> PhoneExists(string phoneNumber)
        { 
            return await UserService.PhoneExists(phoneNumber);
        } 
    }
}