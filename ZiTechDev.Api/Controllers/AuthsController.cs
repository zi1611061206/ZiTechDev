using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using ZiTechDev.Api.Services.Auth;
using ZiTechDev.CommonModel.Requests.Auth;

namespace ZiTechDev.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthsController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthsController(IAuthService authService)
        {
            _authService = authService;
        }

        #region Api/Auths/Register
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
        #endregion

        #region Api/Auths/Login
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
        #endregion

        #region Api/Auths/Login-Warning?userName={userName}&forgotPasswordBaseUrl={forgotPasswordBaseUrl}
        [HttpPost("login-warning")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWarning([FromQuery] string userName, [FromQuery] string forgotPasswordBaseUrl)
        {
            var result = await _authService.LoginWarning(userName, WebUtility.UrlDecode(forgotPasswordBaseUrl));
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }
        #endregion

        #region Api/Auths/Forgot-Password
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.ForgotPassword(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }
        #endregion

        #region Api/Auths/Send-Forgot?email={email}&token={token}&resetPasswordBaseUrl={resetPasswordBaseUrl}
        [HttpPost("send-forgot")]
        [AllowAnonymous]
        public async Task<IActionResult> SendForgotPasswordEmail([FromQuery] string email, [FromQuery] string token, [FromQuery] string resetPasswordBaseUrl)
        {
            var result = await _authService.SendForgotPasswordEmail(email, token, WebUtility.UrlDecode(resetPasswordBaseUrl));
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }
        #endregion

        #region Api/Auths/Unlock-Out?userId={userId}
        [HttpGet("unlock-out")]
        [AllowAnonymous]
        public async Task<IActionResult> UnlockOut([FromQuery] string userId)
        {
            var result = await _authService.UnlockOut(Guid.Parse(userId));
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }
        #endregion

        #region Api/Auths/Reset-Password
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.ResetPassword(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }
        #endregion

        #region Api/Auths/Vertified-Email?userId={userId}&token={token}
        [HttpGet("vertified-email")]
        [AllowAnonymous]
        public async Task<IActionResult> VertifiedEmail([FromQuery] string userId, [FromQuery] string token)
        {
            var result = await _authService.VertifiedEmail(Guid.Parse(userId), token);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }
        #endregion
    }
}
