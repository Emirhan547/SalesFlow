using DocumentFormat.OpenXml.Office2016.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesFlow.Business.Dtos.CustomerDtos;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Core.Constants;
using SalesFlow.Core.Extensions;
using SalesFlow.Core.Paginations;

namespace SalesFlow.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationRequest request)
    {
        var result = await _customerService.GetAllAsync(request);

        return this.ToActionResult(result);
    }
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _customerService.GetByIdAsync(id);

        return this.ToActionResult(result);
    }
    [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager},{Roles.SalesRepresentative}")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerDto dto)
    {
        var result = await _customerService.CreateAsync(dto);

        return this.ToActionResult(result);
    }
    [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager},{Roles.SalesRepresentative}")]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateCustomerDto dto)
    {
        var result = await _customerService.UpdateAsync(dto);

        return this.ToActionResult(result);
    }
    [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager}")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _customerService.DeleteAsync(id);

        return this.ToActionResult(result);
    }
    [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager},{Roles.SalesRepresentative}")]
    [HttpPost("{customerId:int}/tags/{tagId:int}")]
    public async Task<IActionResult> AddTag(int customerId, int tagId)
    {
        var result = await _customerService.AddTagAsync(customerId, tagId);

        return this.ToActionResult(result);
    }
    [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager},{Roles.SalesRepresentative}")]

    [HttpDelete("{customerId:int}/tags/{tagId:int}")]
    public async Task<IActionResult> RemoveTag(int customerId, int tagId)
    {
        var result = await _customerService.RemoveTagAsync(customerId, tagId);

        return this.ToActionResult(result);
    }
    [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager},{Roles.SalesRepresentative}")]

    [HttpGet("{customerId:int}/tags")]
    public async Task<IActionResult> GetTags(int customerId)
    {
        var result = await _customerService.GetTagsAsync(customerId);

        return this.ToActionResult(result);
    }
    [Authorize]
    [HttpGet("export")]
    public async Task<IActionResult> Export()
    {
        byte[] file = await _customerService.ExportAsync();

        string fileName =
      $"Customers_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

        return File(
            file,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName);
    }
    [Authorize]
    [HttpGet("export/pdf")]
    public async Task<IActionResult> ExportPdf()
    {
        byte[] file = await _customerService.ExportPdfAsync();

        return File(
            file,
            FileTypes.Pdf,
            $"Customers_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
    }
}