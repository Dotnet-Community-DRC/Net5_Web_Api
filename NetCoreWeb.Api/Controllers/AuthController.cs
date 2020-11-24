using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreWeb.Api.Services;
using NetCoreWeb.Shared.ViewModels;

namespace NetCoreWeb.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        //api/auth/register

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUserAsync(model);
                if (result.IsSuccess)
                    return Ok(result); // status 200

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid!"); // status code 400
        }
    }
}