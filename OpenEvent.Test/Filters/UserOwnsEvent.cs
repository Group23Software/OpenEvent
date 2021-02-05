using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using OpenEvent.Web.UserOwnsEvent;

namespace OpenEvent.Test.Filters
{
    [TestFixture]
    public class UserOwnsEvent
    {
        private readonly Mock<Web.Services.IUserService> UserServiceMock = new();
        private AuthorizationFilterContext AuthorizationFilterContext;
        private ActionContext ActionContext;
        private readonly Mock<HttpContext> HttpContextMock = new();

        private readonly Guid ValidEventId = new Guid("F41BE850-09E9-452E-9C47-A3787DC3F0D3");
        private readonly Guid ValidUserId = new Guid("648B6902-2593-470B-9203-0001DDA929A1");

        [SetUp]
        public async Task Setup()
        {
            UserServiceMock.Setup(x => x.HostOwnsEvent(ValidEventId, ValidUserId)).ReturnsAsync(true);
            
            ActionContext = new ActionContext(HttpContextMock.Object, new RouteData(), new ActionDescriptor());

            AuthorizationFilterContext = new AuthorizationFilterContext(ActionContext, new List<IFilterMetadata>());
        }

        [Test]
        public async Task ShouldAllowRequest()
        {
            HttpContextMock.Setup(a => a.Request.Headers["eventId"]).Returns(ValidEventId.ToString());
            HttpContextMock.Setup(a => a.Request.Headers["userId"]).Returns(ValidUserId.ToString());
            
            var filter = new UserOwnsEventFilter(UserServiceMock.Object);
            await filter.OnAuthorizationAsync(AuthorizationFilterContext);

            AuthorizationFilterContext.Result.Should().BeNull();
        }
        
        [Test]
        public async Task ShouldDenyNullRequest()
        {
            HttpContextMock.Setup(a => a.Request.Headers["eventId"]).Returns(string.Empty);
            HttpContextMock.Setup(a => a.Request.Headers["userId"]).Returns(string.Empty);
            
            var filter = new UserOwnsEventFilter(UserServiceMock.Object);
            await filter.OnAuthorizationAsync(AuthorizationFilterContext);

            AuthorizationFilterContext.Result.Should().BeOfType<UnauthorizedResult>();
        }
        
        [Test]
        public async Task ShouldDenyRequest()
        {
            HttpContextMock.Setup(a => a.Request.Headers["eventId"]).Returns(new Guid().ToString());
            HttpContextMock.Setup(a => a.Request.Headers["userId"]).Returns(new Guid().ToString());
            
            var filter = new UserOwnsEventFilter(UserServiceMock.Object);
            await filter.OnAuthorizationAsync(AuthorizationFilterContext);

            AuthorizationFilterContext.Result.Should().BeOfType<UnauthorizedResult>();
        }
        
        [Test]
        public async Task ShouldDenyInvalidGuidRequest()
        {
            HttpContextMock.Setup(a => a.Request.Headers["eventId"]).Returns("INVALID");
            HttpContextMock.Setup(a => a.Request.Headers["userId"]).Returns("INVALID");
            
            var filter = new UserOwnsEventFilter(UserServiceMock.Object);
            await filter.OnAuthorizationAsync(AuthorizationFilterContext);

            AuthorizationFilterContext.Result.Should().BeOfType<UnauthorizedResult>();
        }
    }
}