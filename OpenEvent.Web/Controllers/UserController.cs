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
        public async Task<ActionResult<User>> Create([FromBody] NewUserInput newUserInput)
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
                return Ok("Destroyed");
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
    }
}