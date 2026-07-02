using SalesFlow.Entity.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Entity.Entities
{
    public class Note : BaseEntity
    {
        public string Content { get; set; } = null!;

        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public int CreatedById { get; set; }

        public AppUser CreatedBy { get; set; } = null!;
    }
}
