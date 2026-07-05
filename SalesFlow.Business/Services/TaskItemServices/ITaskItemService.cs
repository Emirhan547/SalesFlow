using SalesFlow.Business.Dtos.TaskItemDtos;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.TaskItemServices
{
    public interface ITaskItemService
    {
        Task<Result> CreateAsync(CreateTaskItemDto dto);
        Task<Result> UpdateAsync(UpdateTaskItemDto dto);
        Task<Result> DeleteAsync(int id);
        Task<Result<PagedResult<ResultTaskItemDto>>> GetAllAsync(PaginationRequest request);
        Task<Result<GetByIdTaskItemDto>> GetByIdAsync(int id);
    }
}
