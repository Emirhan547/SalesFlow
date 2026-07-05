using SalesFlow.DataAccess.Context;
using SalesFlow.DataAccess.Repositories.GenericRepositories;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.DataAccess.Repositories.TaskItemRepositories
{
    public class TaskItemRepository : GenericRepository<TaskItem>, ITaskItemRepository
    {
        public TaskItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}
