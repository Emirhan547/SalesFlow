using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesFlow.Business.Dtos.TagDtos;
using SalesFlow.Business.Services.TagServices;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Extensions;
using SalesFlow.Core.Constants;

namespace SalesFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequest request)
        {
            var result = await _tagService.GetAllAsync(request);

            return this.ToActionResult(result);
        }
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _tagService.GetByIdAsync(id);

            return this.ToActionResult(result);
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateTagDto dto)
        {
            var result = await _tagService.CreateAsync(dto);

            return this.ToActionResult(result);
        }
        [Authorize(Roles =Roles.Admin)]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateTagDto dto)
        {
            var result = await _tagService.UpdateAsync(dto);

            return this.ToActionResult(result);
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _tagService.DeleteAsync(id);

            return this.ToActionResult(result);
        }
    }
}