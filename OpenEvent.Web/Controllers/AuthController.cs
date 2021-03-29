using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenEvent.Data.Models.Auth;
using OpenEvent.Data.Models.User;
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
                Logger.LogInformation("Login error {Exception}", e.ToString());
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
                Logger.LogInformation("Authenticate error {Exception}", e);
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
        [AllowAnonymous]
        [HttpPost("updatePassword")]
        public async Task<ActionResult> UpdatePassword([FromBody] UpdatePasswordBody updatePasswordBody)
        {
            try
            {
                Logger.LogInformation("Updating password {Id}", updatePasswordBody.Id);
                await AuthService.UpdatePassword(updatePasswordBody.Id, updatePasswordBody.Password);
                return Ok();
            }
            catch (Exception e)
            {
                Logger.LogInformation("Update password error {Exception}", e);
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
                Logger.LogInformation("Confirming {Id}", id);
                await AuthService.ConfirmEmail(id);
                return Ok("Confirmed");
            }
            catch (Exception e)
            {
                Logger.LogInformation("Confirm email error {Exception}", e);
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
                Logger.LogInformation("{Email} forgetting password", email);
                await AuthService.ForgotPassword(email);
                return Ok();
            }
            catch (Exception e)
            {
                Logger.LogInformation("Forgot email error {Exception}", e);
                return BadRequest(e.Message);
            }
        }
    }
}