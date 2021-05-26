using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZiTechDev.Business.Requests.Role;
using ZiTechDev.Business.Services.Role;

namespace ZiTechDev.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAll();
            return Ok(roles);
        }

        [HttpPost("")]
        public async Task<IActionResult> Get([FromBody] RoleFilter filter)
        {
            var result = await _roleService.Get(filter);
            return Ok(result);
        }

        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetById(Guid roleId)
        {
            var result = await _roleService.GetById(roleId);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] RoleCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _roleService.Create(request);

            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }

        [HttpPut("update/{roleId}")]
        public async Task<IActionResult> Update(Guid roleId, [FromBody] RoleUpdateRequest request)
        {
            request.Id = roleId;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _roleService.Update(request);

            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }

        [HttpDelete("delete/{roleId}")]
        public async Task<IActionResult> Delete(Guid roleId)
        {
            var result = await _roleService.Delete(roleId);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }
    }
}
