using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesFlow.Business.Dtos.DealDtos;
using SalesFlow.Business.Services.DealServices;
using SalesFlow.Core.Paginations;

namespace SalesFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealsController : ControllerBase
    {
        private readonly IDealService _dealService;

        public DealsController(IDealService dealService)
        {
            _dealService = dealService;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequest request)
        {
            return Ok(await _dealService.GetAllAsync(request));
        }
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _dealService.GetByIdAsync(id));
        }
        [Authorize(Roles = "Admin,SalesManager,SalesRepresentative")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateDealDto dto)
        {
            return Ok(await _dealService.CreateAsync(dto));
        }
        [Authorize(Roles = "Admin,SalesManager,SalesRepresentative")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateDealDto dto)
        {
            return Ok(await _dealService.UpdateAsync(dto));
        }
        [Authorize(Roles = "Admin,SalesManager")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _dealService.DeleteAsync(id));
        }
    }
}
