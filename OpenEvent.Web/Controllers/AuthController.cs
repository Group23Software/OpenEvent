using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Models.Auth;
using OpenEvent.Web.Models.User;
using OpenEvent.Web.Services;

namespace OpenEvent.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService AuthService;
        private readonly ILogger<AuthController> Logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            AuthService = authService;
            Logger = logger;
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
                Logger.LogError(e.ToString());
                return Unauthorized(e.Message);
            }
        }

        [HttpPost("authenticateToken")]
        public async Task<ActionResult<UserViewModel>> AuthenticateToken([FromBody] AuthId id)
        {
            try
            {
                var result = await AuthService.Authenticate(id.Id);
                return Ok(result);
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return Unauthorized(e.Message);
            }
        }

        [HttpPost("updatePassword")]
        public async Task<ActionResult> UpdatePassword([FromBody] UpdatePasswordBody updatePasswordBody)
        {
            try
            {
                await AuthService.UpdatePassword(updatePasswordBody.Email, updatePasswordBody.Password);
                return Ok();
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return BadRequest(e.Message);
            }
        }
    }
}