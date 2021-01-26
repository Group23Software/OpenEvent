using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthBody authBody)
        {
            try
            {
                var result = await AuthService.Authenticate(authBody.Email, authBody.Password);
                return Ok(new {token = result});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Unauthorized();
            }
        }
    }

    public class AuthBody
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}