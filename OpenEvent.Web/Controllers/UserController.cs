using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenEvent.Web.Models;
using OpenEvent.Web.Services;

namespace OpenEvent.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService UserService;

        public UserController(IUserService userService)
        {
            UserService = userService;
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
                return BadRequest(e);
            }
        }

        // [HttpPut]
        // public async Task<ActionResult> Update(UserAccountModel user)
        // {
        //     try
        //     {
        //         var result = await UserService.Update(user);
        //         return Ok(result);
        //     }
        //     catch (Exception e)
        //     {
        //         return BadRequest(e);
        //     }
        // }

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
                Console.WriteLine(e);
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
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }
    }

    public class UpdateAvatarBody
    {
        public Guid Id { get; set; }
        public byte[] Avatar { get; set; }
    }

    public class UpdateUserNameBody
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
    }
}