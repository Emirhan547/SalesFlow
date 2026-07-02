using SalesFlow.Entity.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Entity.Entities
{
    public class Meeting : BaseEntity
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? Location { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public int CreatedById { get; set; }

        public AppUser CreatedBy { get; set; } = null!;
    }
}
