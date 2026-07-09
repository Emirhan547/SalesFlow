using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesFlow.Business.Dtos.NoteDtos;
using SalesFlow.Business.Services.NoteServices;
using SalesFlow.Core.Constants;
using SalesFlow.Core.Extensions;
using SalesFlow.Core.Paginations;

namespace SalesFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequest request)
        {
            var result = await _noteService.GetAllAsync(request);

            return this.ToActionResult(result);
        }
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _noteService.GetByIdAsync(id);

            return this.ToActionResult(result);
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager},{Roles.SalesRepresentative}")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateNoteDto dto)
        {
            var result = await _noteService.CreateAsync(dto);

            return this.ToActionResult(result);
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager},{Roles.SalesRepresentative}")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateNoteDto dto)
        {
            var result = await _noteService.UpdateAsync(dto);

            return this.ToActionResult(result);
        }
        [Authorize(Roles = $"{Roles.Admin},{Roles.SalesManager}")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _noteService.DeleteAsync(id);

            return this.ToActionResult(result);
        }
    }
}
