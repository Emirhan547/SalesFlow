using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using TaskStatus = SalesFlow.Entity.Enums.TaskStatus;

namespace SalesFlow.Business.Dtos.TaskItemDtos
{
    public class ResultTaskItemDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public DateTime DueDate { get; set; }

        public TaskPriority Priority { get; set; }

        public TaskStatus Status { get; set; }

        public int CustomerId { get; set; }

        public int? AssignedUserId { get; set; }
    }
}
