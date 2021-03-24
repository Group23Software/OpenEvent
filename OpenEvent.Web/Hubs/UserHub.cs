#nullable enable
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Services;

namespace OpenEvent.Web.Hubs
{
    /// <summary>
    /// Web-socket hub of quick user methods
    /// </summary>
    public class UserHub : Hub
    {
        private readonly ILogger<UserHub> Logger;
        private readonly IUserService UserService;

        /// <inheritdoc />
        public UserHub(ILogger<UserHub> logger, IUserService userService)
        {
            Logger = logger;
            UserService = userService;
        }

        /// <inheritdoc />
        public override async Task OnConnectedAsync()
        {
            Logger.LogInformation("{Id} has connected to user hub",Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        /// <inheritdoc />
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Logger.LogInformation("{Id} has disconnected from user hub",Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Checks if the username exists
        /// </summary>
        /// <param name="username">username to be checked</param>
        /// <returns>true if phone username exists</returns>
        public async Task<bool> UserNameExists(string username)
        { 
            return await UserService.UserNameExists(username);
        }
        
        /// <summary>
        /// Checks if the email exists
        /// </summary>
        /// <param name="email">email to be checked</param>
        /// <returns>true if email exists</returns>
        public async Task<bool> EmailExists(string email)
        { 
            return await UserService.EmailExists(email);
        } 
        
        /// <summary>
        /// Checks if the phone number exists
        /// </summary>
        /// <param name="phoneNumber">phone number to be checked</param>
        /// <returns>true if phone number exists</returns>
        public async Task<bool> PhoneExists(string phoneNumber)
        { 
            return await UserService.PhoneExists(phoneNumber);
        } 
    }
}