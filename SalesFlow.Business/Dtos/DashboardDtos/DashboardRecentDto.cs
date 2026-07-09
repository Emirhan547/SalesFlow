using SalesFlow.Business.Dtos.CustomerDtos;
using SalesFlow.Business.Dtos.DealDtos;
using SalesFlow.Business.Dtos.LeadDtos;
using SalesFlow.Business.Dtos.MeetingDtos;
using SalesFlow.Business.Dtos.TaskItemDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.DashboardDtos
{
    public class DashboardRecentDto
    {
        public List<RecentCustomerDto> Customers { get; set; } = [];

        public List<RecentLeadDto> Leads { get; set; } = [];

        public List<RecentDealDto> Deals { get; set; } = [];

        public List<UpcomingMeetingDto> UpcomingMeetings { get; set; } = [];

        public List<UpcomingTaskDto> UpcomingTasks { get; set; } = [];
    }
}
