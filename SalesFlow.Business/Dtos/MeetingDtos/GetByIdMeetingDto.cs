using SalesFlow.Entity.Enums;

namespace SalesFlow.Business.Dtos.MeetingDtos
{
    public class GetByIdMeetingDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public MeetingType Type { get; set; }

        public MeetingStatus Status { get; set; }

        public string? Location { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; } = null!;

        public int? AssignedUserId { get; set; }

        public string? AssignedUserName { get; set; }
    }
}