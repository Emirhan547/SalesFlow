using SalesFlow.Entity.Common;
using SalesFlow.Entity.Enums;

namespace SalesFlow.Entity.Entities
{
    public class Meeting : BaseEntity
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public MeetingType Type { get; set; }

        public MeetingStatus Status { get; set; }

        public string? Location { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public int? AssignedUserId { get; set; }

        public AppUser? AssignedUser { get; set; }
    }
}