using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesFlow.Business.Dtos.MeetingDtos;
using SalesFlow.Business.Services.MeetingServices;
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

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _meetingService.GetByIdAsync(id);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [Authorize(Roles = "Admin,SalesManager,SalesRepresentative")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateMeetingDto dto)
        {
            var result = await _meetingService.CreateAsync(dto);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [Authorize(Roles = "Admin,SalesManager,SalesRepresentative")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateMeetingDto dto)
        {
            var result = await _meetingService.UpdateAsync(dto);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [Authorize(Roles = "Admin,SalesManager")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _meetingService.DeleteAsync(id);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
