using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZiTechDev.Business.Interfaces;
using ZiTechDev.Business.Requests.Auth;

namespace ZiTechDev.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthsController : ControllerBase
    {
        private readonly IAuthService _userService;
        public AuthsController(IAuthService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm]LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var tokenResult = await _userService.Login(request);
            if (string.IsNullOrEmpty(tokenResult))
            {
                return BadRequest("Username or password is incorrect");
            }
            return Ok(new { token = tokenResult });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Register(request);
            if (!result)
            {
                return BadRequest("Register is unsucessful");
            }
            return Ok();
        }
    }
}
