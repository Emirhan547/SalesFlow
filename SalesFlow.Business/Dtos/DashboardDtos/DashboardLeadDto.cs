using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.DashboardDtos
{
    public class DashboardLeadDto
    {
        public int New { get; set; }

        public int Contacted { get; set; }

        public int Qualified { get; set; }

        public int Converted { get; set; }

        public int Lost { get; set; }
    }
}
