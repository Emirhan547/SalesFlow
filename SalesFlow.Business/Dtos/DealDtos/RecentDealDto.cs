using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.DealDtos
{
    public class RecentDealDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public decimal Amount { get; set; }

        public DealStage Stage { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
