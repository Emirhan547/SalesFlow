using Microsoft.AspNetCore.Identity;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.TaskItemRepositories;
using SalesFlow.Entity.Entities;
using TaskStatus = SalesFlow.Entity.Enums.TaskStatus;

namespace SalesFlow.Business.Services.TaskItemServices
{
    public class TaskItemBusinessRules
    {
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly CustomerBusinessRules _customerBusinessRules;

        public TaskItemBusinessRules(
            ITaskItemRepository taskItemRepository,
            UserManager<AppUser> userManager,
            CustomerBusinessRules customerBusinessRules)
        {
            _taskItemRepository = taskItemRepository;
            _userManager = userManager;
            _customerBusinessRules = customerBusinessRules;
        }

        public async Task<TaskItem> GetTaskItemByIdAsync(
            int id,
            bool tracking = false)
        {
            var task = await _taskItemRepository
                .GetByIdAsync(id, tracking);

            if (task is null)
                throw new NotFoundException(
                    "Task not found.");

            return task;
        }

        public async Task EnsureCustomerExistsAsync(
            int customerId)
        {
            await _customerBusinessRules
                .EnsureCustomerExistsAsync(customerId);
        }

        public async Task EnsureAssignedUserExistsAsync(
            int? userId)
        {
            if (!userId.HasValue)
                return;

            var user = await _userManager
                .FindByIdAsync(
                    userId.Value.ToString());

            if (user is null)
                throw new BusinessException(
                    "Assigned user not found.");
        }

        public async Task EnsureActiveTaskTitleIsUniqueAsync(
            string title,
            int? assignedUserId)
        {
            if (!assignedUserId.HasValue)
                return;

            var exists =
                await _taskItemRepository.AnyAsync(x =>
                    x.AssignedUserId == assignedUserId &&
                    x.Title == title &&
                    x.Status != TaskStatus.Completed &&
                    x.Status != TaskStatus.Cancelled);

            if (exists)
                throw new BusinessException(
                    "An active task with the same title already exists for this user.");
        }

        public async Task EnsureActiveTaskTitleIsUniqueForUpdateAsync(
            int taskId,
            string title,
            int? assignedUserId)
        {
            if (!assignedUserId.HasValue)
                return;

            var exists =
                await _taskItemRepository.AnyAsync(x =>
                    x.Id != taskId &&
                    x.AssignedUserId == assignedUserId &&
                    x.Title == title &&
                    x.Status != TaskStatus.Completed &&
                    x.Status != TaskStatus.Cancelled);

            if (exists)
                throw new BusinessException(
                    "An active task with the same title already exists for this user.");
        }

        public void EnsureStatusTransition(
            TaskStatus current,
            TaskStatus next)
        {
            var valid = current switch
            {
                TaskStatus.Pending =>
                    next is TaskStatus.InProgress
                    or TaskStatus.Cancelled,

                TaskStatus.InProgress =>
                    next is TaskStatus.Completed
                    or TaskStatus.Cancelled,

                TaskStatus.Completed => false,

                TaskStatus.Cancelled => false,

                _ => false
            };

            if (!valid)
                throw new BusinessException(
                    "Invalid task status transition.");
        }
        public void EnsureTaskIsEditable(TaskItem task)
        {
            if (task.Status is TaskStatus.Completed or TaskStatus.Cancelled)
                throw new BusinessException(
                    "Completed or cancelled tasks cannot be updated.");
        }
    }
}