using Microsoft.AspNetCore.Mvc;
using SalesFlow.Business.Dtos.CustomerDtos;
using SalesFlow.Business.Services.CustomerServices;
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

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationRequest request)
    {
        var result = await _customerService.GetAllAsync(request);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _customerService.GetByIdAsync(id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerDto dto)
    {
        var result = await _customerService.CreateAsync(dto);

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateCustomerDto dto)
    {
        var result = await _customerService.UpdateAsync(dto);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _customerService.DeleteAsync(id);

        return Ok(result);
    }
}