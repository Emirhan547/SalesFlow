using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.RealtimeServices
{
    public interface IRealtimeService
    {
        Task ActivityLogCreatedAsync();

        Task DashboardUpdatedAsync();
        Task SendNotificationAsync(int userId,object notification);
    }
}
