using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.MeetingDtos
{
    public class UpcomingMeetingDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public MeetingStatus Status { get; set; }

        public string CustomerName { get; set; } = null!;
    }
}
