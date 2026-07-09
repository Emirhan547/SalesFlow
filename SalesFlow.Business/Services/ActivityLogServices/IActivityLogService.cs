using SalesFlow.Business.Dtos.ActivityLogDtos;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.ActivityLogServices
{
    public interface IActivityLogService
    {
        Task AddAsync(ActivityAction action,string entityName,int entityId,string description, int? userId);
        Task<Result<PagedResult<ResultActivityLogDto>>> GetAllAsync(ActivityLogFilterRequest request);

        Task<Result<GetByIdActivityLogDto>> GetByIdAsync(int id);
    }
}
