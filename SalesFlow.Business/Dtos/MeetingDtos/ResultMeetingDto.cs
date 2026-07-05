using SalesFlow.Entity.Enums;
using SalesFlow.Entity.Enums.SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.MeetingDtos
{
    public class ResultMeetingDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public MeetingType Type { get; set; }

        public MeetingStatus Status { get; set; }

        public int CustomerId { get; set; }

        public int? AssignedUserId { get; set; }
    }
}
