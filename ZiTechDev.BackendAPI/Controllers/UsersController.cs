using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using ZiTechDev.BackendAPI.Engines.Email;
using ZiTechDev.Business.Engines.Email;
using ZiTechDev.Business.Requests.User;
using ZiTechDev.Business.Services.User;

namespace ZiTechDev.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public UsersController(
            IUserService userService, 
            IEmailService emailService, 
            IWebHostEnvironment webHostEnvironment, 
            IConfiguration configuration)
        {
            _userService = userService;
            _emailService = emailService;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        [HttpPost("")]
        public async Task<IActionResult> Get([FromBody] UserFilter filter)
        {
            var result = await _userService.Get(filter);
            return Ok(result);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById(Guid userId)
        {
            var result = await _userService.GetById(userId);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] UserCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.Create(request);

            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            //var confirmationLink = Url.Action("confirm-email", @"api/Auths", new { userName = request.UserName, token = result.ReturnedObject }, Request.Scheme);
            var confirmationLink = "https://localhost:5001/api/Auths/confirm-email?userName=" + request.UserName + "&token=" + result.ReturnedObject;
            
            var email = new EmailItem();
            var name = _configuration.GetValue<string>("EmailSender:Name");
            var address = _configuration.GetValue<string>("EmailSender:Address");
            email.Senders.Add(new EmailBase(name, address));
            email.Receivers.Add(new EmailBase(request.UserName, request.Email));
            var template = new EmailTemplate(_webHostEnvironment.WebRootPath);
            template.EmailConfirmation(confirmationLink);
            email.Subject = template.Subject;
            email.Body = template.Content;
            await _emailService.SendAsync(email);
            return Ok(result.ReturnedObject);
        }

        [HttpPut("update/{userId}")]
        public async Task<IActionResult> Update(Guid userId, [FromBody] UserUpdateRequest request)
        {
            request.Id = userId;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.Update(request);

            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }

        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var result = await _userService.Delete(userId);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }

        [HttpGet("reset-password/{userId}")]
        public async Task<IActionResult> ResetPassword(Guid userId)
        {
            var result = await _userService.ResetPassword(userId);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }
    }
}
