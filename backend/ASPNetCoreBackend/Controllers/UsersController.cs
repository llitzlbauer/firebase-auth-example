using ASPNetCoreBackend.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNetCoreBackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        public readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        public IActionResult Get(Guid id)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;

            if (userId == null) return BadRequest("no user id claim found");
            if (Guid.Parse(userId) != id) return Unauthorized("User is not permitted to access data of antoher user");

            var user = userService.Get(id);

            return Ok(new UserDto
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                UserId = user.Id
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), 200)]
        public async Task<IActionResult> Register([FromBody] RegisterUserModel model)
        {
            var user = await userService.Add(model);
            return Ok(new UserDto
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                UserId = user.Id
            });
        }
    }
}
