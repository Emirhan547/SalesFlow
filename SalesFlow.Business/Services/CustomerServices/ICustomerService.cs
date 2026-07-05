using SalesFlow.Business.Dtos.CustomerDtos;
using SalesFlow.Business.Dtos.TagDtos;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;

namespace SalesFlow.Business.Services.CustomerServices
{
    public interface ICustomerService
    {
        Task<Result> CreateAsync(CreateCustomerDto dto);
        Task<Result> UpdateAsync(UpdateCustomerDto dto);
        Task<Result> DeleteAsync(int id);
        Task<Result> AddTagAsync(int customerId, int tagId);

        Task<Result> RemoveTagAsync(int customerId, int tagId);

        Task<Result<List<ResultTagDto>>> GetTagsAsync(int customerId);
        Task<Result<PagedResult<ResultCustomerDto>>> GetAllAsync(PaginationRequest request);
        Task<Result<GetByIdCustomerDto>> GetByIdAsync(int id);
    }
}
