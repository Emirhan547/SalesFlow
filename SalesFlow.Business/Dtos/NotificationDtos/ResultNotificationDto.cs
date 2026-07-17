using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.NotificationDtos
{
    public class ResultNotificationDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Message { get; set; } = null!;

        public NotificationType Type { get; set; }

        public bool IsRead { get; set; }

        public string? EntityName { get; set; }

        public int? EntityId { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
