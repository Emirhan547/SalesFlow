using SalesFlow.Business.Dtos.AttachmentDtos;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.AttachmentServices
{
    public interface IAttachmentService
    {
        Task<Result> CreateAsync(CreateAttachmentDto dto);
        Task<Result> DeleteAsync(int id);
        Task<Result<PagedResult<ResultAttachmentDto>>> GetAllAsync(PaginationRequest request);
        Task<Result<GetByIdAttachmentDto>> GetByIdAsync(int id);
        Task<DownloadAttachmentDto> DownloadAsync(int id);
        Task<Result<PagedResult<ResultAttachmentDto>>> GetAllAsync(AttachmentFilterRequest request);
    }
}
