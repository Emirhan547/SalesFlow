using SalesFlow.Entity.Common;
using SalesFlow.Entity.Enums;

using TaskStatus = SalesFlow.Entity.Enums.TaskStatus;

namespace SalesFlow.Entity.Entities
{
    public class TaskItem : BaseEntity
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime DueDate { get; set; }

        public TaskPriority Priority { get; set; }

        public TaskStatus Status { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public int? AssignedUserId { get; set; }

        public AppUser? AssignedUser { get; set; }
    }
}
