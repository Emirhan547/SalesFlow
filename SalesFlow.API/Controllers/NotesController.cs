using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesFlow.Business.Dtos.NoteDtos;
using SalesFlow.Business.Services.NoteServices;
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

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _noteService.GetByIdAsync(id);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [Authorize(Roles = "Admin,SalesManager,SalesRepresentative")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateNoteDto dto)
        {
            var result = await _noteService.CreateAsync(dto);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [Authorize(Roles = "Admin,SalesManager,SalesRepresentative")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateNoteDto dto)
        {
            var result = await _noteService.UpdateAsync(dto);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [Authorize(Roles = "Admin,SalesManager")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _noteService.DeleteAsync(id);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
