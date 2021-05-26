using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZiTechDev.Business.Engines.Paginition;
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

        public UsersController(IUserService userService)
        {
            _userService = userService;
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
