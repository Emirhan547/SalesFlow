using SalesFlow.Entity.Common;
using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace SalesFlow.Entity.Entities
{
    public class Customer : BaseEntity
    {

        public CustomerType CustomerType { get; set; }

        public string? CompanyName { get; set; }

        public string ContactFirstName { get; set; } = null!;

        public string ContactLastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string? Website { get; set; }

        public string? TaxNumber { get; set; }

        public string? Address { get; set; }

        public string? Description { get; set; }
        public int? AssignedUserId { get; set; }

        public AppUser? AssignedUser { get; set; }

        // Navigation Properties
        public ICollection<Deal> Deals { get; set; } = new List<Deal>();

        public ICollection<Meeting> Meetings { get; set; } = new List<Meeting>();

        public ICollection<Note> Notes { get; set; } = new List<Note>();

        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

        public ICollection<CustomerTag> CustomerTags { get; set; } = new List<CustomerTag>();
    }
}
