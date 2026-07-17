using SalesFlow.Core.Paginations;
using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.NotificationDtos
{
    public class NotificationFilterRequest : PaginationRequest
    {
        public NotificationType? Type { get; set; }

        public bool? IsRead { get; set; }
    }
}
