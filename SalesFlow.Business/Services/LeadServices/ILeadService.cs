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
        Task<byte[]> ExportAsync();
        Task<byte[]> ExportPdfAsync();
        Task<Result<PagedResult<ResultLeadDto>>> GetAllAsync(PaginationRequest request);
        Task<Result> ConvertAsync(int leadId, ConvertLeadDto dto);
        Task<Result<GetByIdLeadDto>> GetByIdAsync(int id);
        Task<Result<string>> GenerateSummaryAsync(int leadId);
        Task<Result<LeadScoreResponse>> GetLeadScoreAsync(int leadId);
    }
}
