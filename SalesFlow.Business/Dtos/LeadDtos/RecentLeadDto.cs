using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.LeadDtos
{
    public class RecentLeadDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string? CompanyName { get; set; }

        public LeadStatus Status { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
