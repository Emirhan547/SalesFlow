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
        Task<Result<string>> GenerateInsightsAsync(int customerId);
        Task<Result> RemoveTagAsync(int customerId, int tagId);
        Task<byte[]> ExportAsync();
        Task<byte[]> ExportPdfAsync();
        Task<Result<List<ResultTagDto>>> GetTagsAsync(int customerId);
        Task<Result<PagedResult<ResultCustomerDto>>> GetAllAsync(CustomerFilterRequest request);
        Task<Result<GetByIdCustomerDto>> GetByIdAsync(int id);
        Task<Result<string>> GenerateFollowUpEmailAsync(
    int customerId,
    GenerateFollowUpEmailDto dto);
    }
}
