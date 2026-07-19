using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Moq;
using SalesFlow.Business.Dtos.TaskItemDtos;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Business.Services.NotificationServices;
using SalesFlow.Business.Services.RealtimeServices;
using SalesFlow.Business.Services.TaskItemServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Repositories.TagRepositories;
using SalesFlow.DataAccess.Repositories.TaskItemRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;
using System.Linq.Expressions;
using TaskStatus = SalesFlow.Entity.Enums.TaskStatus;

namespace SalesFlow.UnitTests.Services.TaskItemServices;

public class TaskItemService_UpdateDeleteTests
{
    private readonly Mock<ITaskItemRepository> _taskRepositoryMock = new();
    private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
    private readonly Mock<ITagRepository> _tagRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly Mock<IValidator<CreateTaskItemDto>> _createValidatorMock = new();
    private readonly Mock<IValidator<UpdateTaskItemDto>> _updateValidatorMock = new();

    private readonly Mock<IActivityLogService> _activityLogServiceMock = new();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
    private readonly Mock<IRealtimeService> _realtimeServiceMock = new();
    private readonly Mock<INotificationService> _notificationServiceMock = new();

    private readonly Mock<UserManager<AppUser>> _userManagerMock;

    private readonly TaskItemService _service;

    public TaskItemService_UpdateDeleteTests()
    {
        var userStore = new Mock<IUserStore<AppUser>>();

        _userManagerMock = new Mock<UserManager<AppUser>>(
            userStore.Object,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null);

        _currentUserServiceMock
            .Setup(x => x.UserId)
            .Returns(1);

        _currentUserServiceMock
            .Setup(x => x.IsInRole("Admin"))
            .Returns(true);

        var authBusinessRules =
            new AuthBusinessRules(
                _currentUserServiceMock.Object);

        var customerBusinessRules =
            new CustomerBusinessRules(
                _customerRepositoryMock.Object,
                _tagRepositoryMock.Object,
                _currentUserServiceMock.Object,
                authBusinessRules);

        var taskBusinessRules =
            new TaskItemBusinessRules(
                _taskRepositoryMock.Object,
                _userManagerMock.Object,
                customerBusinessRules,
                authBusinessRules);

        _service = new TaskItemService(
            _taskRepositoryMock.Object,
            _unitOfWorkMock.Object,
            taskBusinessRules,
            _createValidatorMock.Object,
            _updateValidatorMock.Object,
            _activityLogServiceMock.Object,
            _currentUserServiceMock.Object,
            _realtimeServiceMock.Object,
            _notificationServiceMock.Object);
    }
    [Fact]
    public async Task UpdateAsync_Should_Update_Task_Successfully()
    {
        // Arrange
        var dto = new UpdateTaskItemDto
        {
            Id = 1,
            Title = "Updated Task",
            CustomerId = 1,
            AssignedUserId = 1,
            Status = TaskStatus.InProgress
        };

        var task = new TaskItem
        {
            Id = 1,
            Title = "Old",
            CustomerId = 1,
            AssignedUserId = 1,
            Status = TaskStatus.Pending
        };

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _taskRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(task);

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(new Customer());

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(new AppUser());

        _taskRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<TaskItem, bool>>>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.UpdateAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _taskRepositoryMock.Verify(
            x => x.Update(task),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);

        _activityLogServiceMock.Verify(
            x => x.AddAsync(
                ActivityAction.Update,
                nameof(TaskItem),
                task.Id,
                It.IsAny<string>(),
                1),
            Times.Once);

        _realtimeServiceMock.Verify(
            x => x.DashboardUpdatedAsync(),
            Times.Once);
    }
    [Fact]
    public async Task UpdateAsync_Should_Update_Properties()
    {
        var dto = new UpdateTaskItemDto
        {
            Id = 1,
            Title = "Updated",
            Description = "Description",
            CustomerId = 2,
            AssignedUserId = 3,
            Priority = TaskPriority.High,
            Status = TaskStatus.InProgress
        };

        var task = new TaskItem
        {
            Id = 1,
            Status = TaskStatus.Pending
        };

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _taskRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(task);

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(2, false))
            .ReturnsAsync(new Customer());

        _userManagerMock
            .Setup(x => x.FindByIdAsync("3"))
            .ReturnsAsync(new AppUser());

        _taskRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<TaskItem, bool>>>()))
            .ReturnsAsync(false);

        await _service.UpdateAsync(dto);

        task.Title.Should().Be(dto.Title);
        task.Description.Should().Be(dto.Description);
        task.CustomerId.Should().Be(dto.CustomerId);
        task.AssignedUserId.Should().Be(dto.AssignedUserId);
        task.Priority.Should().Be(dto.Priority);
        task.Status.Should().Be(dto.Status);
    }
    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Validation_Fails()
    {
        var dto = new UpdateTaskItemDto();

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult(new[]
            {
            new ValidationFailure("Title", "Required")
            }));

        Func<Task> act = () => _service.UpdateAsync(dto);

        await act.Should()
            .ThrowAsync<ValidationException>();
    }
    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Task_Not_Found()
    {
        var dto = new UpdateTaskItemDto
        {
            Id = 1
        };

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _taskRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync((TaskItem?)null);

        Func<Task> act = () => _service.UpdateAsync(dto);

        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("Task not found.");
    }
    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Task_Is_Not_Editable()
    {
        // Arrange
        var dto = new UpdateTaskItemDto
        {
            Id = 1,
            CustomerId = 1,
            AssignedUserId = 1,
            Status = TaskStatus.Completed
        };

        var task = new TaskItem
        {
            Id = 1,
            CustomerId = 1,
            AssignedUserId = 1,
            Status = TaskStatus.Completed
        };

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _taskRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(task);

        // Act
        Func<Task> act = () => _service.UpdateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("Completed or cancelled tasks cannot be updated.");
    }
    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Duplicate_Title()
    {
        // Arrange
        var dto = new UpdateTaskItemDto
        {
            Id = 1,
            Title = "Duplicate",
            CustomerId = 1,
            AssignedUserId = 1,
            Status = TaskStatus.Pending
        };

        var task = new TaskItem
        {
            Id = 1,
            Status = TaskStatus.Pending,
            AssignedUserId = 1
        };

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _taskRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(task);

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(new Customer());

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(new AppUser());

        _taskRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<TaskItem, bool>>>()))
            .ReturnsAsync(true);

        // Act
        Func<Task> act = () => _service.UpdateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("An active task with the same title already exists for this user.");
    }
    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Invalid_Status_Transition()
    {
        // Arrange
        var dto = new UpdateTaskItemDto
        {
            Id = 1,
            CustomerId = 1,
            AssignedUserId = 1,
            Status = TaskStatus.Completed
        };

        var task = new TaskItem
        {
            Id = 1,
            CustomerId = 1,
            AssignedUserId = 1,
            Status = TaskStatus.Pending
        };

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _taskRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(task);

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(new Customer());

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(new AppUser());

        _taskRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<TaskItem, bool>>>()))
            .ReturnsAsync(false);

        // Act
        Func<Task> act = () => _service.UpdateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("Invalid task status transition.");
    }
    [Fact]
    public async Task UpdateAsync_Should_Create_Assignment_Notification()
    {
        // Arrange
        var dto = new UpdateTaskItemDto
        {
            Id = 1,
            Title = "Task",
            CustomerId = 1,
            AssignedUserId = 2,
            Status = TaskStatus.Pending
        };

        var task = new TaskItem
        {
            Id = 1,
            Title = "Task",
            CustomerId = 1,
            AssignedUserId = 1,
            Status = TaskStatus.Pending
        };

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _taskRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(task);

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(new Customer());

        _userManagerMock
            .Setup(x => x.FindByIdAsync("2"))
            .ReturnsAsync(new AppUser());

        _taskRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<TaskItem, bool>>>()))
            .ReturnsAsync(false);

        // Act
        await _service.UpdateAsync(dto);

        // Assert
        _notificationServiceMock.Verify(
            x => x.AddAsync(
                2,
                "Task Assigned",
                It.IsAny<string>(),
                NotificationType.Info,
                nameof(TaskItem),
                It.IsAny<int>()),
            Times.Once);
    }
    [Fact]
    public async Task DeleteAsync_Should_Delete_Task_Successfully()
    {
        // Arrange
        var task = new TaskItem
        {
            Id = 1,
            Title = "Task",
            AssignedUserId = 1
        };

        _taskRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(task);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _taskRepositoryMock.Verify(
            x => x.Delete(task),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);

        _activityLogServiceMock.Verify(
            x => x.AddAsync(
                ActivityAction.Delete,
                nameof(TaskItem),
                task.Id,
                It.IsAny<string>(),
                1),
            Times.Once);

        _realtimeServiceMock.Verify(
            x => x.DashboardUpdatedAsync(),
            Times.Once);
    }
    [Fact]
    public async Task DeleteAsync_Should_Throw_When_Task_Not_Found()
    {
        // Arrange
        _taskRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync((TaskItem?)null);

        // Act
        Func<Task> act = () => _service.DeleteAsync(1);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("Task not found.");
    }
    [Fact]
    public async Task DeleteAsync_Should_Throw_When_User_Has_No_Access()
    {
        // Arrange
        _currentUserServiceMock
            .Setup(x => x.IsInRole("Admin"))
            .Returns(false);

        _currentUserServiceMock
            .Setup(x => x.IsInRole("SalesManager"))
            .Returns(false);

        _currentUserServiceMock
            .Setup(x => x.UserId)
            .Returns(1);

        var task = new TaskItem
        {
            Id = 1,
            AssignedUserId = 5
        };

        _taskRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(task);

        // Act
        Func<Task> act = () => _service.DeleteAsync(1);

        // Assert
        await act.Should()
            .ThrowAsync<ForbiddenException>()
            .WithMessage("You are not authorized to access this record.");
    }
    [Fact]
    public async Task DeleteAsync_Should_Create_ActivityLog()
    {
        // Arrange
        var task = new TaskItem
        {
            Id = 1,
            Title = "Demo Task",
            AssignedUserId = 1
        };

        _taskRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(task);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _activityLogServiceMock.Verify(
            x => x.AddAsync(
                ActivityAction.Delete,
                nameof(TaskItem),
                task.Id,
                It.IsAny<string>(),
                1),
            Times.Once);
    }
    [Fact]
    public async Task DeleteAsync_Should_Call_DashboardUpdated()
    {
        // Arrange
        var task = new TaskItem
        {
            Id = 1,
            AssignedUserId = 1
        };

        _taskRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(task);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _realtimeServiceMock.Verify(
            x => x.DashboardUpdatedAsync(),
            Times.Once);
    }
}