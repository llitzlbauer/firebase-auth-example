using ASPNetCoreBackend.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ASPNetCoreBackend.Controllers
{
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
        public IActionResult Get(Guid id)
        {
            var user = userService.Get(id);
            return Ok(new UserDto
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                UserId = user.Id
            });
        }

        [HttpPost]
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
