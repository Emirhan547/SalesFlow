using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.DashboardDtos
{
    public class DashboardTaskDto
    {
        public int Pending { get; set; }

        public int InProgress { get; set; }

        public int Completed { get; set; }

        public int Cancelled { get; set; }
    }
}
