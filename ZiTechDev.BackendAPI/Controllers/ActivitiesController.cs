using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZiTechDev.Business.Requests.Activity;
using ZiTechDev.Business.Services.Activities;

namespace ZiTechDev.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivitiesController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get([FromQuery] ActivityFilter filter)
        {
            var result = await _activityService.GetAll(filter);
            return Ok(result.ReturnedObject);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] ActivityCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _activityService.Create(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] ActivityUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _activityService.Update(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _activityService.Delete(id);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }
    }
}
