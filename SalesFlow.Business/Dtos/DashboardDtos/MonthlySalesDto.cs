using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.DashboardDtos
{
    public class MonthlySalesDto
    {
        public string Month { get; set; } = string.Empty;

        public decimal Amount { get; set; }
    }
}
