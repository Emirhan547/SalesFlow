using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.DealDtos
{
    public class CreateDealDto
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Amount { get; set; }

        public DateTime? ExpectedCloseDate { get; set; }

        public int CustomerId { get; set; }

        public int? AssignedUserId { get; set; }
    }
}
