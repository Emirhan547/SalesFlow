using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesFlow.Business.Dtos.MeetingDtos;
using SalesFlow.Business.Services.MeetingServices;
using SalesFlow.Core.Constants;
using SalesFlow.Core.Extensions;
using SalesFlow.Core.Paginations;

namespace SalesFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        private readonly IMeetingService _meetingService;

        public MeetingsController(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequest request)
        {
            var result = await _meetingService.GetAllAsync(request);

            return this.ToActionResult(result);
        }
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _meetingService.GetByIdAsync(id);

            return this.ToActionResult(result);
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager},{Roles.SalesRepresentative}")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateMeetingDto dto)
        {
            var result = await _meetingService.CreateAsync(dto);

            return this.ToActionResult(result);
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager},{Roles.SalesRepresentative}")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateMeetingDto dto)
        {
            var result = await _meetingService.UpdateAsync(dto);

            return this.ToActionResult(result);
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager}")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _meetingService.DeleteAsync(id);

            return this.ToActionResult(result);
        }
        [Authorize]
        [HttpGet("availability")]
        public async Task<IActionResult> CheckAvailability(
    [FromQuery] int assignedUserId,
    [FromQuery] DateTime startDate,
    [FromQuery] DateTime endDate,
    [FromQuery] int? meetingId = null)
        {
            var result =
                await _meetingService.CheckAvailabilityAsync(
                    assignedUserId,
                    startDate,
                    endDate,
                    meetingId);

            return this.ToActionResult(result);
        }
    }
}
