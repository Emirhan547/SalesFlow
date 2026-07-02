using SalesFlow.Entity.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Entity.Entities
{
    public class CustomerTag : BaseEntity
    {
        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public int TagId { get; set; }

        public Tag Tag { get; set; } = null!;
    }
}
