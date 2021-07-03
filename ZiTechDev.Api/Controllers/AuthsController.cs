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

        #region Api/Auths/Register?activatedEmailBaseUrl={activatedEmailBaseUrl}
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromQuery] string activatedEmailBaseUrl, [FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.Register(activatedEmailBaseUrl,request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }
        #endregion

        #region Api/Auths/Validate-UserName
        [HttpPost("validate-username")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateUserName([FromBody] LoginUserNameRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.ValidateUserName(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }
        #endregion

        #region Api/Auths/Validate-Login?resetPasswordBaseUrl={resetPasswordBaseUrl}
        [HttpPost("validate-login")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateLogin([FromQuery] string resetPasswordBaseUrl, [FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.ValidateLogin(WebUtility.UrlDecode(resetPasswordBaseUrl), request);
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

        #region Api/Auths/Send-To-Authenticator?userName={userName}&provider={provider}
        [HttpGet("send-to-authenticator")]
        [AllowAnonymous]
        public async Task<IActionResult> SendToAuthenticator([FromQuery] string userName, string provider)
        {
            var result = await _authService.SendToAuthenticator(userName, provider);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }
        #endregion

        #region Api/Auths/Authenticate-2FA
        [HttpPost("authenticate-2fa")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate2FA([FromBody] Authenticate2FARequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.Authenticate2FA(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }
        #endregion

        #region Api/Auths/Forgot-Password?resetPasswordBaseUrl={resetPasswordBaseUrl}
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromQuery] string resetPasswordBaseUrl, [FromBody] ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.ForgotPassword(resetPasswordBaseUrl, request);
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

        #region Api/Auths/Vertified-Change-Email?userId={userId}&token={token}&newEmail={newEmail}
        [HttpGet("vertified-change-email")]
        [AllowAnonymous]
        public async Task<IActionResult> VertifiedChangeEmail([FromQuery] string userId, [FromQuery] string token, [FromQuery] string newEmail)
        {
            var result = await _authService.VertifiedChangeEmail(Guid.Parse(userId), token, newEmail);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }
        #endregion

        #region Api/Auths/Activated-Email?userId={userId}&token={token}
        [HttpGet("activated-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ActivatedEmail([FromQuery] string userId, [FromQuery] string token)
        {
            var result = await _authService.ActivatedEmail(Guid.Parse(userId), token);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }
        #endregion
    }
}
