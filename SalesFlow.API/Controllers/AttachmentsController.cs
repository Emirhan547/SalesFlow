using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesFlow.Business.Dtos.AttachmentDtos;
using SalesFlow.Business.Services.AttachmentServices;
using SalesFlow.Core.Paginations;

namespace SalesFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentsController : ControllerBase
    {
        private readonly IAttachmentService _attachmentService;

        public AttachmentsController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequest request)
        {
            var result = await _attachmentService.GetAllAsync(request);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _attachmentService.GetByIdAsync(id);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [Authorize(Roles = "Admin,SalesManager,SalesRepresentative")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateAttachmentDto dto)
        {
            var result = await _attachmentService.CreateAsync(dto);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [Authorize(Roles = "Admin,SalesManager,SalesRepresentative")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateAttachmentDto dto)
        {
            var result = await _attachmentService.UpdateAsync(dto);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [Authorize(Roles = "Admin,SalesManager")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _attachmentService.DeleteAsync(id);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}