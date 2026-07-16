using SalesFlow.Core.Paginations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.AttachmentDtos
{
    public class AttachmentFilterRequest : PaginationRequest
    {
        public int? CustomerId { get; set; }
    }
}
