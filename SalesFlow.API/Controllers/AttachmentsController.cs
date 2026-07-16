using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesFlow.Business.Dtos.AttachmentDtos;
using SalesFlow.Business.Services.AttachmentServices;
using SalesFlow.Core.Constants;
using SalesFlow.Core.Extensions;
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

            return this.ToActionResult(result);
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _attachmentService.GetByIdAsync(id);

            return this.ToActionResult(result);
        }

        [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager},{Roles.SalesRepresentative}")]
        [Consumes("multipart/form-data")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateAttachmentDto dto)
        {
            var result = await _attachmentService.CreateAsync(dto);

            return this.ToActionResult(result);
        }



        [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager}")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _attachmentService.DeleteAsync(id);

            return this.ToActionResult(result);
        }
        [Authorize]
        [HttpGet("download/{id:int}")]
        public async Task<IActionResult> Download(int id)
        {
            var result = await _attachmentService.DownloadAsync(id);

            return File(
                result.FileBytes,
                result.ContentType,
                result.FileName);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] AttachmentFilterRequest request)
        {
            var result = await _attachmentService.GetAllAsync(request);
            return this.ToActionResult(result);
        }
    }
}