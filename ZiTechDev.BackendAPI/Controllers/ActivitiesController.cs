using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZiTechDev.Business.Interfaces;
using ZiTechDev.Business.Requests.Activity;

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
            var activity = await _activityService.GetAll(filter);
            return Ok(activity);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] ActivityCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var activityId = await _activityService.Create(request);
            if (activityId == 0)
            {
                return BadRequest();
            }
            return Ok(activityId);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] ActivityUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedResult = await _activityService.Update(request);
            if (affectedResult == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var affectedResult = await _activityService.Delete(id);
            if (affectedResult == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
