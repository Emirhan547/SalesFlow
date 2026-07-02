using SalesFlow.Business.Dtos.CustomerDtos;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;

namespace SalesFlow.Business.Services.CustomerServices
{
    public interface ICustomerService
    {
        Task<Result> CreateAsync(CreateCustomerDto dto);
        Task<Result> UpdateAsync(UpdateCustomerDto dto);
        Task<Result> DeleteAsync(int id);
        Task<Result<PagedResult<ResultCustomerDto>>> GetAllAsync(PaginationRequest request);
        Task<Result<GetByIdCustomerDto>> GetByIdAsync(int id);
    }
}
