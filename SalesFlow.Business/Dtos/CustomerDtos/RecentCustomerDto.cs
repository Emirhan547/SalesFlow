using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.CustomerDtos
{
    public class RecentCustomerDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string? CompanyName { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
