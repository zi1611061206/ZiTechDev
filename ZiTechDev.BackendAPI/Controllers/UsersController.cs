using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var user = await _userService.GetAll(filter);
            return Ok(user);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] UserCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = await _userService.Create(request);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }
            return Ok(userId);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Update(request);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userService.Delete(id);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
