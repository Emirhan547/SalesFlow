using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.ActivityLogDtos
{
    public class ResultActivityLogDto
    {
        public int Id { get; set; }

        public ActivityAction Action { get; set; }

        public string EntityName { get; set; } = null!;

        public int EntityId { get; set; }

        public string Description { get; set; } = null!;

        public int? UserId { get; set; }

        public string? UserName { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
