using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationRequest request)
    {
        var result = await _customerService.GetAllAsync(request);

        return Ok(result);
    }
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _customerService.GetByIdAsync(id);

        return Ok(result);
    }
    [Authorize(Roles = "Admin,SalesManager,SalesRepresentative")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerDto dto)
    {
        var result = await _customerService.CreateAsync(dto);

        return Ok(result);
    }
    [Authorize(Roles = "Admin,SalesManager,SalesRepresentative")]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateCustomerDto dto)
    {
        var result = await _customerService.UpdateAsync(dto);

        return Ok(result);
    }
    [Authorize(Roles = "Admin,SalesManager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _customerService.DeleteAsync(id);

        return Ok(result);
    }
    [HttpPost("{customerId:int}/tags/{tagId:int}")]
    public async Task<IActionResult> AddTag(int customerId, int tagId)
    {
        var result = await _customerService.AddTagAsync(customerId, tagId);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{customerId:int}/tags/{tagId:int}")]
    public async Task<IActionResult> RemoveTag(int customerId, int tagId)
    {
        var result = await _customerService.RemoveTagAsync(customerId, tagId);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{customerId:int}/tags")]
    public async Task<IActionResult> GetTags(int customerId)
    {
        var result = await _customerService.GetTagsAsync(customerId);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}