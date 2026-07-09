using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesFlow.Business.Dtos.ActivityLogDtos;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Core.Extensions;
using SalesFlow.Core.Paginations;

namespace SalesFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityLogsController : ControllerBase
    {
        private readonly IActivityLogService _activityLogService;

        public ActivityLogsController(IActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ActivityLogFilterRequest request)
        {
            var result = await _activityLogService.GetAllAsync(request);
            return this.ToActionResult(result);
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _activityLogService.GetByIdAsync(id);

            return this.ToActionResult(result);
        }
    }
}
