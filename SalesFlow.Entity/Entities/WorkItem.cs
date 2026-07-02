using SalesFlow.Entity.Common;
using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Entity.Entities
{
    public class WorkItem : BaseEntity
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime DueDate { get; set; }

        public WorkItemPriority Priority { get; set; }

        public WorkItemStatus Status { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public int AssignedUserId { get; set; }

        public AppUser AssignedUser { get; set; } = null!;
    }
}
