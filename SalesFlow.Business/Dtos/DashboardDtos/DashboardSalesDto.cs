using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.DashboardDtos
{
    public class DashboardSalesDto
    {
        public decimal PipelineAmount { get; set; }

        public decimal WonAmount { get; set; }

        public int WonDeals { get; set; }

        public int LostDeals { get; set; }

        public int ActiveDeals { get; set; }
        public List<MonthlySalesDto> MonthlySales { get; set; } = [];
    }
}
