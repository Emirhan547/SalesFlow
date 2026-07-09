using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.TaskItemServices
{
    public class UpcomingTaskDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public DateTime DueDate { get; set; }

        public TaskStatus Status { get; set; }

        public string CustomerName { get; set; } = null!;
    }
}
