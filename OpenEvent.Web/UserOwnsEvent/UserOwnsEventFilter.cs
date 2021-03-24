using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OpenEvent.Web.Services;

namespace OpenEvent.Web.UserOwnsEvent
{
    /// <summary>
    /// Filter that determines if a user owns an event based on request headers
    /// </summary>
    public class UserOwnsEventFilter : IAsyncAuthorizationFilter
    {
        private readonly IUserService UserService;
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="userService"></param>
        public UserOwnsEventFilter(IUserService userService)
        {
            UserService = userService;
        }

        /// <summary>
        /// Intercepts incoming http request to determine if the user owns the event.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            IHeaderDictionary headers = context.HttpContext.Request.Headers;
            
            // Extracts headers from the request
            string eventId = headers["eventId"];
            string userId = headers["userId"];

            // If headers don't exist return unauthorised
            if (eventId == null || userId == null || eventId == string.Empty || userId == string.Empty)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            try
            {
                var userOwns = await UserService.HostOwnsEvent(Guid.Parse(eventId), Guid.Parse(userId));

                // If the user doesn't own the event return unauthorised
                if (!userOwns)
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            catch
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}