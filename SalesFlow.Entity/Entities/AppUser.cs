using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Entity.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? ProfileImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Lead> AssignedLeads { get; set; } = new List<Lead>();

        public ICollection<Deal> Deals { get; set; } = new List<Deal>();

        public ICollection<Meeting> Meetings { get; set; } = new List<Meeting>();

        public ICollection<WorkItem> WorkItems { get; set; } = new List<WorkItem>();

        public ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}