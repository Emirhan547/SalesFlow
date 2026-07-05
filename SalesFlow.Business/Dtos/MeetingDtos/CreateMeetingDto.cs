using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.MeetingDtos
{
    public class CreateMeetingDto
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public MeetingType Type { get; set; }

        public string? Location { get; set; }

        public int CustomerId { get; set; }

        public int? AssignedUserId { get; set; }
    }
}
