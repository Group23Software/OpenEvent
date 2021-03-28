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
    /// <summary>
    /// API controller for all authentication related endpoints.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService AuthService;
        private readonly ILogger<AuthController> Logger;

        /// <summary>
        /// AuthController default constructor.
        /// </summary>
        /// <param name="authService"></param>
        /// <param name="logger"></param>
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            AuthService = authService;
            Logger = logger;
        }

        /// <summary>
        /// Endpoint to login a user.
        /// </summary>
        /// <param name="loginBody"></param>
        /// <returns>
        /// ActionResult of <see cref="UserViewModel"/> representing basic user information.
        /// Unauthorized if any exceptions are caught.
        /// </returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserViewModel>> Login([FromBody] LoginBody loginBody)
        {
            try
            {
                var result = await AuthService.Login(loginBody.Email, loginBody.Password, loginBody.Remember);
                return Ok(result);
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return Unauthorized(e.Message);
            }
        }

        /// <summary>
        /// Endpoint to authenticate user once logged-in.
        /// </summary>
        /// <param name="authBody"></param>
        /// <returns>
        /// ActionResult of <see cref="UserViewModel"/> representing basic user information.
        /// Unauthorized if any exceptions are caught.
        /// </returns>
        [HttpPost("authenticateToken")]
        public async Task<ActionResult<UserViewModel>> Authenticate([FromBody] AuthBody authBody)
        {
            try
            {
                var result = await AuthService.Authenticate(authBody.Id);
                return Ok(result);
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return Unauthorized(e.Message);
            }
        }

        /// <summary>
        /// Endpoint to update a user's password.
        /// </summary>
        /// <param name="updatePasswordBody"></param>
        /// <returns>
        /// ActionResult if password has been updated.
        /// BadRequest if any exceptions are caught.
        /// </returns>
        [HttpPost("updatePassword")]
        public async Task<ActionResult> UpdatePassword([FromBody] UpdatePasswordBody updatePasswordBody)
        {
            try
            {
                await AuthService.UpdatePassword(updatePasswordBody.Id, updatePasswordBody.Password);
                return Ok();
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return BadRequest(e.Message);
            }
        }
        
        /// <summary>
        /// Endpoint to confirm a user email
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ActionResult of user</returns>
        [AllowAnonymous]
        [HttpGet("confirm")]
        public async Task<ActionResult<UserViewModel>> Confirm(Guid id)
        {
            try
            {
                await AuthService.ConfirmEmail(id);
                return Ok("Confirmed");
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return Unauthorized(e.Message);
            }
        }
        
        /// <summary>
        /// Endpoint to send a forgot password email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>ActionResult of user</returns>
        [AllowAnonymous]
        [HttpGet("forgot")]
        public async Task<ActionResult<UserViewModel>> Forgot(string email)
        {
            try
            {
                await AuthService.ForgotPassword(email);
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