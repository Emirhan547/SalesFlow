using SalesFlow.Business.Dtos.DealDtos;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.DealServices
{
    public interface IDealService
    {
        Task<Result> CreateAsync(CreateDealDto dto);

        Task<Result> UpdateAsync(UpdateDealDto dto);

        Task<Result> DeleteAsync(int id);
        Task<byte[]> ExportAsync();
        Task<byte[]> ExportPdfAsync();
        Task<Result<PagedResult<ResultDealDto>>> GetAllAsync(PaginationRequest request);

        Task<Result<GetByIdDealDto>> GetByIdAsync(int id);
    }
}
