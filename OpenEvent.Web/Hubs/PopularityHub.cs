using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace OpenEvent.Web.Hubs
{
    public class PopularityHub : Hub
    {
        private readonly ILogger<PopularityHub> Logger;

        public PopularityHub(ILogger<PopularityHub> logger)
        {
            Logger = logger;
        }
        
        public override async Task OnConnectedAsync()
        {
            Logger.LogInformation("{Id} has connected",Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Logger.LogInformation("{Id} has disconnected",Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        // public async Task SendEvents(List<PopularEventViewModel> events)
        // {
        //     Logger.LogInformation("Sending events");
        //     await Clients.All.SendAsync("events", events);
        // }
    }
}