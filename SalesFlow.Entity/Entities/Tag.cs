using SalesFlow.Entity.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Entity.Entities
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; } = null!;

        public ICollection<CustomerTag> CustomerTags { get; set; } = new List<CustomerTag>();
    }
}
