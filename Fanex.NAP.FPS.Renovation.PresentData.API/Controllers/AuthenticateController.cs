using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.NAP.FPS.Renovation.Model;
using Fanex.NAP.FPS.Renovation.PresentData.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.NAP.FPS.Renovation.PresentData.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateService _authService;

        public AuthenticateController(IAuthenticateService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost, Route("request")]
        public IActionResult RequestToken([FromBody] TokenRequest request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string token;

            if (_authService.IsAuthenticated(request, out token))
            {
                return Ok(token);
            }

            return BadRequest("Invalid Request");
        }

        [AllowAnonymous]
        [HttpGet, Route("request")]
        public IActionResult RequestToken()
        {
            return Ok("testing");
        }
    }
}