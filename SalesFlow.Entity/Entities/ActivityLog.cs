using SalesFlow.Entity.Common;
using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Entity.Entities
{
    public class ActivityLog : BaseEntity
    {
        public ActivityAction Action { get; set; }

        public string EntityName { get; set; } = null!;

        public int EntityId { get; set; }

        public string Description { get; set; } = null!;

        public int? UserId { get; set; }

        public AppUser? User { get; set; }
    }
}
