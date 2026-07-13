using SalesFlow.Business.Dtos.ActivityLogDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.DashboardDtos
{
    public class DashboardDto
    {
        public DashboardSummaryDto Summary { get; set; } = null!;

        public DashboardSalesDto Sales { get; set; } = null!;

        public DashboardLeadDto Leads { get; set; } = null!;

        public DashboardTaskDto Tasks { get; set; } = null!;

        public DashboardMeetingDto Meetings { get; set; } = null!;

        public DashboardRecentDto Recent { get; set; } = null!;
        public List<ResultActivityLogDto> RecentActivities { get; set; } = [];
    }
}
