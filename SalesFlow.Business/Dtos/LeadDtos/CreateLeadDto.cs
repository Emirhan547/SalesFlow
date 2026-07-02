using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.LeadDtos
{
    public class CreateLeadDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? CompanyName { get; set; }
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? Website { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public LeadStatus Status { get; set; }
        public LeadSource Source { get; set; }
        public int? AssignedUserId { get; set; }
    }
}
