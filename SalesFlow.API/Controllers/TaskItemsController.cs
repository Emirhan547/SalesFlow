using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesFlow.Business.Dtos.TaskItemDtos;
using SalesFlow.Business.Services.TaskItemServices;
using SalesFlow.Core.Constants;
using SalesFlow.Core.Extensions;
using SalesFlow.Core.Paginations;

namespace SalesFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemsController : ControllerBase
    {
        private readonly ITaskItemService _taskItemService;

        public TaskItemsController(ITaskItemService taskItemService)
        {
            _taskItemService = taskItemService;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequest request)
        {
            var result = await _taskItemService.GetAllAsync(request);

            return this.ToActionResult(result);
        }
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _taskItemService.GetByIdAsync(id);

            return this.ToActionResult(result);
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager},{Roles.SalesRepresentative}")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskItemDto dto)
        {
            var result = await _taskItemService.CreateAsync(dto);

            return this.ToActionResult(result);
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager},{Roles.SalesRepresentative}")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateTaskItemDto dto)
        {
            var result = await _taskItemService.UpdateAsync(dto);

            return this.ToActionResult(result);
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager}")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _taskItemService.DeleteAsync(id);

            return this.ToActionResult(result);
        }
    }
}
