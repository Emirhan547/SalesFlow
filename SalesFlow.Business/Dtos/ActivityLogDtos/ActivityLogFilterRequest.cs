using SalesFlow.Core.Paginations;
using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.ActivityLogDtos
{
    public class ActivityLogFilterRequest : PaginationRequest
    {
        public ActivityAction? Action { get; set; }

        public string? EntityName { get; set; }

        public int? UserId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
