using System.Threading.Tasks;
using Fanex.NAP.FPS.Renovation.Model;
using Fanex.NAP.FPS.Renovation.PresentData.API.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.NAP.FPS.Renovation.PresentData.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CreateUserModel command)
        {
            await _userService.RegisterAsync(command.Email, command.Password, command.Name);

            return Json("Register Account successful");
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] AuthenticateUserModel command)
        {
            return Json(await _userService.LoginAsync(command.Email, command.Password));
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult TestAuthenticate()
        {
            return new JsonResult("Test successfully");
        }

    }
}