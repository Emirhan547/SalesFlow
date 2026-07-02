using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesFlow.Business.Dtos.LeadDtos;
using SalesFlow.Business.Services.LeadServices;
using SalesFlow.Core.Paginations;

namespace SalesFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeadsController : ControllerBase
    {
        private readonly ILeadService _leadService;

        public LeadsController(ILeadService leadService)
        {
            _leadService = leadService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequest request)
        {
            var leads = await _leadService.GetAllAsync(request);
            return Ok(leads);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var leads = await _leadService.GetByIdAsync(id);
            return Ok(leads);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLeadDto dto)
        {
            var lead = await _leadService.CreateAsync(dto);
            return Ok(lead);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateLeadDto dto)
        {
            var lead = await _leadService.UpdateAsync(dto);
            return Ok(lead);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var lead= await _leadService.DeleteAsync(id);
            return Ok(lead);
        }
    }
}
