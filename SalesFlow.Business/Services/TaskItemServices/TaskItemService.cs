using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SalesFlow.Business.Dtos.TaskItemDtos;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Business.Services.NotificationServices;
using SalesFlow.Business.Services.RealtimeServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using SalesFlow.DataAccess.Repositories.TaskItemRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;
using TaskStatus = SalesFlow.Entity.Enums.TaskStatus;

namespace SalesFlow.Business.Services.TaskItemServices
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TaskItemBusinessRules _businessRules;
        private readonly IValidator<CreateTaskItemDto> _createValidator;
        private readonly IValidator<UpdateTaskItemDto> _updateValidator;
        private readonly IActivityLogService _activityLogService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRealtimeService _realtimeService;
        private readonly INotificationService _notificationService;
        public TaskItemService(ITaskItemRepository taskItemRepository, IUnitOfWork unitOfWork, TaskItemBusinessRules businessRules, IValidator<CreateTaskItemDto> createValidator, IValidator<UpdateTaskItemDto> updateValidator, IActivityLogService activityLogService, ICurrentUserService currentUserService, IRealtimeService realtimeService, INotificationService notificationService)
        {
            _taskItemRepository = taskItemRepository;
            _unitOfWork = unitOfWork;
            _businessRules = businessRules;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _activityLogService = activityLogService;
            _currentUserService = currentUserService;
            _realtimeService = realtimeService;
            _notificationService = notificationService;
        }

        public async Task<Result> CreateAsync(CreateTaskItemDto dto)
        {
            await ValidateAndThrowAsync(_createValidator, dto);
            await _businessRules.EnsureCustomerExistsAsync( dto.CustomerId);
            await _businessRules.EnsureAssignedUserExistsAsync(dto.AssignedUserId);

            await _businessRules.EnsureActiveTaskTitleIsUniqueAsync(dto.Title, dto.AssignedUserId);

            var task = dto.Adapt<TaskItem>();

            task.Status = TaskStatus.Pending;

            await _taskItemRepository.AddAsync(task);

            await _unitOfWork.SaveChangesAsync();

            await _activityLogService.AddAsync(ActivityAction.Create, nameof(TaskItem),task.Id,$"Task '{task.Title}' created.",_currentUserService.UserId);

            if (task.AssignedUserId.HasValue)
            {
                await _notificationService.AddAsync(task.AssignedUserId.Value,"New Task Assigned",$"Task '{task.Title}' has been assigned to you.",NotificationType.Info,nameof(TaskItem),task.Id);
            }
            await _realtimeService.DashboardUpdatedAsync();
            return Result.Success("Task created successfully.");
        }

        public async Task<Result> UpdateAsync(UpdateTaskItemDto dto)
        {
            await ValidateAndThrowAsync(_updateValidator, dto);

            var task =
                await _businessRules.GetTaskItemByIdAsync(
                    dto.Id,
                    true);
            _businessRules.EnsureUserCanModify(task);
            _businessRules.EnsureTaskIsEditable(task);

            await _businessRules.EnsureCustomerExistsAsync(
                dto.CustomerId);

            await _businessRules.EnsureAssignedUserExistsAsync(
                dto.AssignedUserId);

            await _businessRules.EnsureActiveTaskTitleIsUniqueForUpdateAsync(
                dto.Id,
                dto.Title,
                dto.AssignedUserId);

            if (task.Status != dto.Status)
            {
                _businessRules.EnsureStatusTransition(
                    task.Status,
                    dto.Status);
            }

            int? previousAssignedUserId =
                task.AssignedUserId;

            dto.Adapt(task);

            _taskItemRepository.Update(task);

            await _activityLogService.AddAsync(
                ActivityAction.Update,
                nameof(TaskItem),
                task.Id,
                $"Task '{task.Title}' updated.",
                _currentUserService.UserId);

            await _unitOfWork.SaveChangesAsync();

            if (
                task.AssignedUserId.HasValue &&
                task.AssignedUserId != previousAssignedUserId
            )
            {
                await _notificationService.AddAsync(
                    task.AssignedUserId.Value,
                    "Task Assigned",
                    $"Task '{task.Title}' has been assigned to you.",
                    NotificationType.Info,
                    nameof(TaskItem),
                    task.Id);
            }

            await _realtimeService.DashboardUpdatedAsync();

            return Result.Success(
                "Task updated successfully.");
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var task = await _businessRules.GetTaskItemByIdAsync(id, true);
            _businessRules.EnsureUserCanModify(task);
            _taskItemRepository.Delete(task);
            await _activityLogService.AddAsync(ActivityAction.Delete,nameof(TaskItem),task.Id, $"Task '{task.Title}' deleted.", _currentUserService.UserId);
            await _unitOfWork.SaveChangesAsync();
            await _realtimeService.DashboardUpdatedAsync();
            return Result.Success("Task deleted successfully.");
        }

        public async Task<Result<PagedResult<ResultTaskItemDto>>> GetAllAsync(PaginationRequest request)
        {
            IQueryable<TaskItem> query = _taskItemRepository.GetAll();

            if (!_currentUserService.IsInRole("Admin") &&
                !_currentUserService.IsInRole("SalesManager"))
            {
                query = query.Where(x =>
                    x.AssignedUserId == _currentUserService.UserId);
            }

            var tasks = await query
                .ProjectToType<ResultTaskItemDto>()
                .ToPagedResultAsync(request);

            return Result<PagedResult<ResultTaskItemDto>>
                .Success(tasks);
        }

        public async Task<Result<GetByIdTaskItemDto>> GetByIdAsync(int id)
        {
            TaskItem? task = await _taskItemRepository
                .GetAll()
                .Include(x => x.Customer)
                .Include(x => x.AssignedUser)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (task is null)
            {
                return Result<GetByIdTaskItemDto>.Failure(
                    "Task not found.");
            }
            _businessRules.EnsureUserCanModify(task);
            GetByIdTaskItemDto dto = new()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority,
                Status = task.Status,
                CustomerId = task.CustomerId,

                CustomerName = task.Customer.CompanyName
                    ?? $"{task.Customer.ContactFirstName} {task.Customer.ContactLastName}",

                AssignedUserId = task.AssignedUserId,

                AssignedUserName = task.AssignedUser is null
                    ? null
                    : $"{task.AssignedUser.FirstName} {task.AssignedUser.LastName}"
            };

            return Result<GetByIdTaskItemDto>.Success(dto);
        }
        private static async Task ValidateAndThrowAsync<TDto>(IValidator<TDto> validator, TDto dto)
        {
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }
    }
}
