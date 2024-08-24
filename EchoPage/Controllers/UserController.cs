using EchoPage.Interface;
using EchoPage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EchoPage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices services;
        public UserController(IUserServices services)
        {
            this.services = services;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(Users user)
        {
            var response = await services.Register(user);
            if (response)
            {
                return Ok("user created successfully");
            }
            return BadRequest("something went wrong");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(Users user)
        {
            var response = await services.Login(user);
            if (response == null)
            {
                return BadRequest("something went wrong");
            }
            return Ok(response);
        }
    }
}
