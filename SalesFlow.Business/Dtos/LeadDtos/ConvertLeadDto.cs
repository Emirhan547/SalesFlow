using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.LeadDtos
{
    public class ConvertLeadDto
    {
        public CustomerType CustomerType { get; set; }
        public bool CreateInitialDeal { get; set; }
        public bool CreateInitialTask { get; set; }
        public bool CreateInitialMeeting { get; set; }
    }
}
