using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesFlow.Business.Dtos.LeadDtos;
using SalesFlow.Business.Services.LeadServices;
using SalesFlow.Core.Constants;
using SalesFlow.Core.Extensions;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using SalesFlow.Entity.Entities;

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
        [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager}")]
        [HttpPost("{id:int}/convert")]
        public async Task<IActionResult> Convert(int id, ConvertLeadDto dto)
        {
            var result = await _leadService.ConvertAsync(id, dto);

            return this.ToActionResult(result);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequest request)
        {
            var leads = await _leadService.GetAllAsync(request);
            return this.ToActionResult(leads);
        }
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var leads = await _leadService.GetByIdAsync(id);
            return this.ToActionResult(leads);
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager},{Roles.SalesRepresentative}")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateLeadDto dto)
        {
            var lead = await _leadService.CreateAsync(dto);
            return this.ToActionResult(lead);
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager},{Roles.SalesRepresentative}")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateLeadDto dto)
        {
            var lead = await _leadService.UpdateAsync(dto);
            return this.ToActionResult(lead);
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager}")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var lead= await _leadService.DeleteAsync(id);
            return this.ToActionResult(lead);
        }
        [Authorize]
        [HttpGet("export")]
        public async Task<IActionResult> Export()
        {
            byte[] file = await _leadService.ExportAsync();

            return File(
                file,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Leads_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }
        [Authorize]
        [HttpGet("export/pdf")]
        public async Task<IActionResult> ExportPdf()
        {
            byte[] pdf = await _leadService.ExportPdfAsync();

            return File(
                pdf,
                FileTypes.Pdf,
                $"Leads_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }
    }
}
