using SalesFlow.Business.Dtos.DashboardDtos;
using SalesFlow.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.DashboardServices
{
    public interface IDashboardService
    {
        Task<Result<DashboardDto>> GetDashboardAsync();
    }
}
