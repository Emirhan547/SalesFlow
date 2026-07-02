using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.LeadDtos
{
    public class ResultLeadDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? CompanyName { get; set; }
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public LeadStatus Status { get; set; }
        public LeadSource Source { get; set; }
        public int? AssignedUserId { get; set; }
    }
}
