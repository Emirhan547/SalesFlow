using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesFlow.Business.Dtos.DealDtos;
using SalesFlow.Business.Services.DealServices;
using SalesFlow.Core.Constants;
using SalesFlow.Core.Extensions;
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
            var result = await _dealService.GetAllAsync(request);

            return this.ToActionResult(result);
        }
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _dealService.GetByIdAsync(id);

            return this.ToActionResult(result);
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager},{Roles.SalesRepresentative}")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateDealDto dto)
        {
            var result = await _dealService.CreateAsync(dto);

            return this.ToActionResult(result);
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager},{Roles.SalesRepresentative}")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateDealDto dto)
        {
            var result = await _dealService.UpdateAsync(dto);

            return this.ToActionResult(result);
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager}")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _dealService.DeleteAsync(id);

            return this.ToActionResult(result);
        }
        [Authorize]
        [HttpGet("export")]
        public async Task<IActionResult> Export()
        {
            byte[] file = await _dealService.ExportAsync();

            return File(
                file,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Leads_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }
        [Authorize]
        [HttpGet("export/pdf")]
        public async Task<IActionResult> ExportPdf()
        {
            byte[] pdf = await _dealService.ExportPdfAsync();

            return File(
                pdf,
                FileTypes.Pdf,
                $"Deals_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }
    }
    }

