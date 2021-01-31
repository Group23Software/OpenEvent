using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenEvent.Web.Models.User;
using OpenEvent.Web.Services;

namespace OpenEvent.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService UserService;
        private readonly ILogger<UserController> Logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            UserService = userService;
            Logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<UserViewModel>> Create([FromBody] NewUserInput newUserInput)
        {
            try
            {
                var result = await UserService.Create(newUserInput);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Destroy(Guid id)
        {
            try
            {
                await UserService.Destroy(id);
                return Ok();
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }

        [HttpGet("Account")]
        public async Task<ActionResult<UserAccountModel>> GetAccountUser(Guid id)
        {
            try
            {
                var result = await UserService.Get(id);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }

        [HttpGet("UserNameExists")]
        public async Task<ActionResult<bool>> UserNameExists(string username)
        {
            return Ok(await UserService.UserNameExists(username));
        }

        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> EmailExists(string email)
        {
            return Ok(await UserService.EmailExists(email));
        }

        [HttpGet("PhoneExists")]
        public async Task<ActionResult<bool>> PhoneExists(string phoneNumber)
        {
            return Ok(await UserService.PhoneExists(phoneNumber));
        }

        [HttpPost("updateUserName")]
        public async Task<IActionResult> UpdateUserName([FromBody] UpdateUserNameBody updateUserNameBody)
        {
            try
            {
                var result = await UserService.UpdateUserName(updateUserNameBody.Id, updateUserNameBody.UserName);
                return Ok(new
                {
                    username = result
                });
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }

        [HttpPost("updateAvatar")]
        public async Task<ActionResult<string>> UpdateAvatar([FromBody] UpdateAvatarBody updateAvatarBody)
        {
            try
            {
                var result = await UserService.UpdateAvatar(updateAvatarBody.Id, updateAvatarBody.Avatar);
                return Ok(new {avatar = result});
            }
            catch (Exception e)
            {
                Logger.LogError(e.ToString());
                return BadRequest(e);
            }
        }
    }
}