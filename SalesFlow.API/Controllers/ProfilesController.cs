using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesFlow.Business.Dtos.ProfileDtos;
using SalesFlow.Business.Services.ProfileServices;
using SalesFlow.Core.Extensions;
using System.Security.Claims;
namespace SalesFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfilesController(
            IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            int userId = int.Parse(
     User.FindFirstValue(ClaimTypes.NameIdentifier)!
 );

            var result =
                await _profileService
                    .GetProfileAsync(userId);

            return this.ToActionResult(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(
            UpdateProfileDto dto)
        {
            int userId = int.Parse(
     User.FindFirstValue(ClaimTypes.NameIdentifier)!
 );

            var result =
                await _profileService
                    .UpdateProfileAsync(
                        userId,
                        dto
                    );

            return this.ToActionResult(result);
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(
            ChangePasswordDto dto)
        {
            int userId = int.Parse(
     User.FindFirstValue(ClaimTypes.NameIdentifier)!
 );

            var result =
                await _profileService
                    .ChangePasswordAsync(
                        userId,
                        dto
                    );

            return this.ToActionResult(result);
        }
    }
}