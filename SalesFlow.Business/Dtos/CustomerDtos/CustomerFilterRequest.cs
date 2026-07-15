using SalesFlow.Core.Paginations;
using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.CustomerDtos
{
    public class CustomerFilterRequest : PaginationRequest
    {
        public string? Search { get; set; }

        public CustomerType? CustomerType { get; set; }

        public int? AssignedUserId { get; set; }
    }
}
