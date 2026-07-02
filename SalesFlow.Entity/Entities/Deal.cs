using SalesFlow.Entity.Common;
using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Entity.Entities
{
    public class Deal : BaseEntity
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Amount { get; set; }

        public DateTime? ExpectedCloseDate { get; set; }

        public DealStage Stage { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public int AssignedUserId { get; set; }

        public AppUser AssignedUser { get; set; } = null!;
    }
}
