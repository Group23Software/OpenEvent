using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Models;
using OpenEvent.Web.Services;

namespace OpenEvent.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private IAuthService AuthService;

        public AuthController(IAuthService authService)
        {
            AuthService = authService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<UserViewModel>> Authenticate([FromBody] AuthBody authBody)
        {
            try
            {
                var result = await AuthService.Authenticate(authBody.Email, authBody.Password, authBody.Remember);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Unauthorized(e.Message);
            }
        }

        [HttpPost("authenticateToken")]
        public async Task<ActionResult<UserViewModel>> AuthenticateToken([FromBody] AuthId id)
        {
            Console.WriteLine("Authenticating token");
            try
            {
                var result = await AuthService.Authenticate(id.Id);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Unauthorized(e.Message);
            }
        }
    }

    public class AuthId
    {
        public Guid Id { get; set; }
    }

    public class AuthBody
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
    }
}