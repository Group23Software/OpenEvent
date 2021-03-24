#nullable enable
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace OpenEvent.Web.Hubs
{
    /// <summary>
    /// Web-socket hub for exposing the popularity system
    /// </summary>
    public class PopularityHub : Hub
    {
        private readonly ILogger<PopularityHub> Logger;

        /// <inheritdoc />
        public PopularityHub(ILogger<PopularityHub> logger)
        {
            Logger = logger;
        }

        /// <inheritdoc />
        public override async Task OnConnectedAsync()
        {
            Logger.LogInformation("{Id} has connected",Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        /// <inheritdoc />
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Logger.LogInformation("{Id} has disconnected",Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}