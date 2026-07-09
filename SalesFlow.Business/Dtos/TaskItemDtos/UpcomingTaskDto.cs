

using SalesFlow.Entity.Enums;

namespace SalesFlow.Business.Dtos.TaskItemDtos
{
    public class UpcomingTaskDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public DateTime DueDate { get; set; }

        public Entity.Enums.TaskStatus Status { get; set; }

        public string CustomerName { get; set; } = null!;
    }
}
