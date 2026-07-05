using SalesFlow.Business.Dtos.NoteDtos;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.NoteServices
{
    public interface INoteService
    {
        Task<Result> CreateAsync(CreateNoteDto dto);
        Task<Result> UpdateAsync(UpdateNoteDto dto);
        Task<Result> DeleteAsync(int id);
        Task<Result<PagedResult<ResultNoteDto>>> GetAllAsync(PaginationRequest request);
        Task<Result<GetByIdNoteDto>> GetByIdAsync(int id);
    }
}
