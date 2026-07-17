using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.DealDtos
{
    public class GetByIdDealDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Amount { get; set; }

        public DateTime? ExpectedCloseDate { get; set; }

        public DealStage Stage { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; } = null!;

        public int? AssignedUserId { get; set; }

        public string? AssignedUserName { get; set; }
    }
}
