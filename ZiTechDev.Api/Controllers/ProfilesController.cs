using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ZiTechDev.Api.Services.Profile;
using ZiTechDev.CommonModel.Requests.Profile;

namespace ZiTechDev.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfilesController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        #region Api/Profiles/Get?userId={userId}
        [HttpGet("profile/get")]
        public async Task<IActionResult> GetProfile(Guid userId)
        {
            var result = await _profileService.GetProfile(userId);
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        #endregion

        #region Api/Profiles/Edit?userId={userId}
        [HttpPut("profile/edit")]
        public async Task<IActionResult> EditProfile(Guid userId, [FromBody] ProfileEditRequest request)
        {
            request.Id = userId;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _profileService.EditProfile(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }
        #endregion

        #region Api/Profiles/Change-Password?userId={userId}
        [HttpPut("profile/change-password")]
        public async Task<IActionResult> ChangePassword(Guid userId, [FromBody] ChangePasswordRequest request)
        {
            request.Id = userId;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _profileService.ChangePassword(request);

            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.ReturnedObject);
        }
        #endregion
    }
}
