using FluentValidation;
using Mapster;
using SalesFlow.Business.Dtos.TaskItemDtos;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using SalesFlow.DataAccess.Repositories.TaskItemRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
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

        public TaskItemService(ITaskItemRepository taskItemRepository,IUnitOfWork unitOfWork,TaskItemBusinessRules businessRules,IValidator<CreateTaskItemDto> createValidator,IValidator<UpdateTaskItemDto> updateValidator)
        {
            _taskItemRepository = taskItemRepository;
            _unitOfWork = unitOfWork;
            _businessRules = businessRules;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<Result> CreateAsync(CreateTaskItemDto dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);
            await _businessRules.EnsureCustomerExistsAsync(dto.CustomerId);
            await _businessRules.EnsureAssignedUserExistsAsync(dto.AssignedUserId);
            await _businessRules.EnsureActiveTaskTitleIsUniqueAsync(dto.Title,dto.AssignedUserId);
            var task = dto.Adapt<TaskItem>();
            task.Status = TaskStatus.Pending;
            await _taskItemRepository.AddAsync(task);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Task created successfully.");
        }

        public async Task<Result> UpdateAsync(UpdateTaskItemDto dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);
            var task = await _businessRules.GetTaskItemByIdAsync(dto.Id, true);
            await _businessRules.EnsureCustomerExistsAsync(dto.CustomerId);
            await _businessRules.EnsureAssignedUserExistsAsync(dto.AssignedUserId);
            await _businessRules.EnsureActiveTaskTitleIsUniqueForUpdateAsync( dto.Id, dto.Title,dto.AssignedUserId);
            _businessRules.EnsureStatusChanged(task.Status, dto.Status);
            _businessRules.EnsureStatusTransition( task.Status,dto.Status);
            dto.Adapt(task);
            _taskItemRepository.Update(task);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success("Task updated successfully.");
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var task = await _businessRules.GetTaskItemByIdAsync(id, true);
            _taskItemRepository.Delete(task);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Task deleted successfully.");
        }

        public async Task<Result<PagedResult<ResultTaskItemDto>>> GetAllAsync(PaginationRequest request)
        {
            var tasks = await _taskItemRepository.GetAll() .ProjectToType<ResultTaskItemDto>() .ToPagedResultAsync(request);
            return Result<PagedResult<ResultTaskItemDto>>.Success(tasks);
        }

        public async Task<Result<GetByIdTaskItemDto>> GetByIdAsync(int id)
        {
            var task = await _businessRules.GetTaskItemByIdAsync(id);
            var dto = task.Adapt<GetByIdTaskItemDto>();
            return Result<GetByIdTaskItemDto>.Success(dto);
        }
    }
}
