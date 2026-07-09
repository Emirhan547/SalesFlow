using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.DashboardDtos
{
    public class DashboardMeetingDto
    {
        public int Today { get; set; }

        public int ThisWeek { get; set; }

        public int Scheduled { get; set; }

        public int Completed { get; set; }
    }
}
