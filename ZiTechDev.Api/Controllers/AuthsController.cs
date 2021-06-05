using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ZiTechDev.Api.Services.Auth;
using ZiTechDev.Api.Services.User;
using ZiTechDev.CommonModel.Requests.Auth;

namespace ZiTechDev.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthsController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthsController(
            IAuthService authService,
            IUserService userService,
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.Login(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.Register(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }

        [HttpGet("profile/{userId}")]
        public async Task<IActionResult> GetProfile(Guid userId)
        {
            var result = await _userService.GetById(userId);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPut("profile/edit/{userId}")]
        public async Task<IActionResult> EditProfile(Guid userId, [FromBody] EditProfileRequest request)
        {
            request.Id = userId;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.EditProfile(request);

            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }

        [HttpPut("change-password/{userId}")]
        public async Task<IActionResult> ChangePassword(Guid userId, [FromBody] ChangePasswordRequest request)
        {
            request.Id = userId;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.ChangePassword(request);

            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }

        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userName, [FromQuery] string token)
        {
            var result = await _authService.ConfirmEmail(userName, token);
            var request = _httpContextAccessor.HttpContext.Request;
            var domain = $"{request.Scheme}://{request.Host}";
            if (!result.IsSuccessed)
            {
                return Redirect(domain + "/EmailTemplates/EmailConfirmationFailed.html");
            }
            return Redirect(domain + "/EmailTemplates/EmailConfirmationSuccessed.html");
        }
    }
}
