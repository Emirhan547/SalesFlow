using SalesFlow.Business.Dtos.TagDtos;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.TagServices
{
    public interface ITagService
    {
        Task<Result> CreateAsync(CreateTagDto dto);
        Task<Result> UpdateAsync(UpdateTagDto dto);
        Task<Result> DeleteAsync(int id);
        Task<Result<PagedResult<ResultTagDto>>> GetAllAsync(PaginationRequest request);
        Task<Result<GetByIdTagDto>> GetByIdAsync(int id);
    }
}
