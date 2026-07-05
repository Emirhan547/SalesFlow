using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.TaskItemDtos
{
    public class CreateTaskItemDto
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime DueDate { get; set; }

        public TaskPriority Priority { get; set; }

        public int CustomerId { get; set; }

        public int? AssignedUserId { get; set; }
    }
}
