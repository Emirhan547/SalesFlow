using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.DashboardDtos
{
    public class DashboardSummaryDto
    {
        public int TotalCustomers { get; set; }

        public int TotalLeads { get; set; }

        public int TotalDeals { get; set; }

        public int TotalMeetings { get; set; }

        public int TotalTasks { get; set; }
    }
}
