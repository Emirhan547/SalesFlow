using SalesFlow.Business.Dtos.LeadDtos;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;


namespace SalesFlow.Business.Services.LeadServices
{
    public interface ILeadService
    {
        Task<Result> CreateAsync(CreateLeadDto dto);

        Task<Result> UpdateAsync(UpdateLeadDto dto);

        Task<Result> DeleteAsync(int id);

        Task<Result<PagedResult<ResultLeadDto>>> GetAllAsync(PaginationRequest request);

        Task<Result<GetByIdLeadDto>> GetByIdAsync(int id);
    }
}
