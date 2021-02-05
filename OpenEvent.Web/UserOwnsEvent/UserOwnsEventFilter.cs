using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Services;

namespace OpenEvent.Web.UserOwnsEvent
{
    public class UserOwnsEventFilter : IAsyncAuthorizationFilter
    {
        private readonly IUserService UserService;
        public UserOwnsEventFilter(IUserService userService)
        {
            UserService = userService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            Debug.WriteLine(context);
            IHeaderDictionary headers = context.HttpContext.Request.Headers;
            string eventId = headers["eventId"];
            string userId = headers["userId"];
            if (eventId == null || userId == null || eventId == string.Empty || userId == string.Empty)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            try
            {
                var userOwns = await UserService.HostOwnsEvent(Guid.Parse(eventId), Guid.Parse(userId));

                if (!userOwns)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
            catch
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}