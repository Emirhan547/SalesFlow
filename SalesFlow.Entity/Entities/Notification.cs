using SalesFlow.Entity.Common;
using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Entity.Entities
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; } = null!;

        public string Message { get; set; } = null!;

        public NotificationType Type { get; set; }

        public bool IsRead { get; set; }

        public int UserId { get; set; }

        public AppUser User { get; set; } = null!;

        public string? EntityName { get; set; }

        public int? EntityId { get; set; }
    }
}
