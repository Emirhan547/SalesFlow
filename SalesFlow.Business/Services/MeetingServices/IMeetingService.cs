using SalesFlow.Business.Dtos.MeetingDtos;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.MeetingServices
{
    public interface IMeetingService
    {
        Task<Result> CreateAsync(CreateMeetingDto dto);

        Task<Result> UpdateAsync(UpdateMeetingDto dto);

        Task<Result> DeleteAsync(int id);

        Task<Result<PagedResult<ResultMeetingDto>>> GetAllAsync(PaginationRequest request);

        Task<Result<GetByIdMeetingDto>> GetByIdAsync(int id);
        Task<Result<bool>> CheckAvailabilityAsync(
    int assignedUserId,
    DateTime startDate,
    DateTime endDate,
    int? meetingId = null);
    }
}
