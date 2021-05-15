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

        [HttpPost("")]
        public async Task<IActionResult> Get([FromBody] ActivityFilter filter)
        {
            var result = await _activityService.Get(filter);
            return Ok(result.ReturnedObject);
        }

        [HttpGet("{activityId}")]
        public async Task<IActionResult> GetById(int activityId)
        {
            var result = await _activityService.GetById(activityId);
            if (result.IsSuccessed)
            {
                return Ok(result.ReturnedObject);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ActivityCreateRequest request)
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

        [HttpPut("update/{activityId}")]
        public async Task<IActionResult> Update(int activityId, [FromBody] ActivityUpdateRequest request)
        {
            request.Id = activityId;
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

        [HttpDelete("delete/{activityId}")]
        public async Task<IActionResult> Delete(int activityId)
        {
            var result = await _activityService.Delete(activityId);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }
    }
}
