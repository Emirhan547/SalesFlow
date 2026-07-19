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

public class TaskItemService_CreateTests
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

    public TaskItemService_CreateTests()
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

        var authBusinessRules = new AuthBusinessRules(
            _currentUserServiceMock.Object);

        var customerBusinessRules = new CustomerBusinessRules(
            _customerRepositoryMock.Object,
            _tagRepositoryMock.Object,
            _currentUserServiceMock.Object,
            authBusinessRules);

        var taskBusinessRules = new TaskItemBusinessRules(
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
    public async Task CreateAsync_Should_Create_Task_Successfully()
    {
        // Arrange
        var dto = new CreateTaskItemDto
        {
            Title = "Follow Customer",
            CustomerId = 1,
            AssignedUserId = 1
        };

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(new Customer { Id = 1 });

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(new AppUser { Id = 1 });

        _taskRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<TaskItem, bool>>>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _taskRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<TaskItem>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);

        _activityLogServiceMock.Verify(
            x => x.AddAsync(
                ActivityAction.Create,
                nameof(TaskItem),
                It.IsAny<int>(),
                It.IsAny<string>(),
                1),
            Times.Once);

        _realtimeServiceMock.Verify(
            x => x.DashboardUpdatedAsync(),
            Times.Once);
    }
    [Fact]
    public async Task CreateAsync_Should_Set_Status_To_Pending()
    {
        // Arrange
        var dto = new CreateTaskItemDto
        {
            Title = "Task",
            CustomerId = 1,
            AssignedUserId = 1
        };

        TaskItem? createdTask = null;

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(new Customer());

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(new AppUser());

        _taskRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<TaskItem, bool>>>()))
            .ReturnsAsync(false);

        _taskRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<TaskItem>()))
            .Callback<TaskItem>(t => createdTask = t);

        // Act
        await _service.CreateAsync(dto);

        // Assert
        createdTask.Should().NotBeNull();
        createdTask!.Status.Should().Be(TaskStatus.Pending);
    }
    [Fact]
    public async Task CreateAsync_Should_Map_Dto_To_Task()
    {
        // Arrange
        var dto = new CreateTaskItemDto
        {
            Title = "Important Task",
            Description = "Description",
            CustomerId = 5,
            AssignedUserId = 2
        };

        TaskItem? createdTask = null;

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(5, false))
            .ReturnsAsync(new Customer());

        _userManagerMock
            .Setup(x => x.FindByIdAsync("2"))
            .ReturnsAsync(new AppUser());

        _taskRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<TaskItem, bool>>>()))
            .ReturnsAsync(false);

        _taskRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<TaskItem>()))
            .Callback<TaskItem>(x => createdTask = x);

        // Act
        await _service.CreateAsync(dto);

        // Assert
        createdTask.Should().NotBeNull();
        createdTask!.Title.Should().Be(dto.Title);
        createdTask.Description.Should().Be(dto.Description);
        createdTask.CustomerId.Should().Be(dto.CustomerId);
        createdTask.AssignedUserId.Should().Be(dto.AssignedUserId);
    }
    [Fact]
    public async Task CreateAsync_Should_Throw_When_Validation_Fails()
    {
        // Arrange
        var dto = new CreateTaskItemDto();

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult(new[]
            {
            new ValidationFailure("Title", "Required")
            }));

        // Act
        Func<Task> act = () => _service.CreateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<ValidationException>();
    }
    [Fact]
    public async Task CreateAsync_Should_Throw_When_Customer_Not_Found()
    {
        // Arrange
        var dto = new CreateTaskItemDto
        {
            Title = "Task",
            CustomerId = 1,
            AssignedUserId = 1
        };

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync((Customer?)null);

        // Act
        Func<Task> act = () => _service.CreateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("Customer not found.");
    }
    [Fact]
    public async Task CreateAsync_Should_Throw_When_Assigned_User_Not_Found()
    {
        // Arrange
        var dto = new CreateTaskItemDto
        {
            Title = "Task",
            CustomerId = 1,
            AssignedUserId = 1
        };

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(new Customer { Id = 1 });

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync((AppUser?)null);

        // Act
        Func<Task> act = () => _service.CreateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("Assigned user not found.");
    }
    [Fact]
    public async Task CreateAsync_Should_Throw_When_Duplicate_Title()
    {
        // Arrange
        var dto = new CreateTaskItemDto
        {
            Title = "Task",
            CustomerId = 1,
            AssignedUserId = 1
        };

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(new Customer { Id = 1 });

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(new AppUser { Id = 1 });

        _taskRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<TaskItem, bool>>>()))
            .ReturnsAsync(true);

        // Act
        Func<Task> act = () => _service.CreateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("An active task with the same title already exists for this user.");
    }
    [Fact]
    public async Task CreateAsync_Should_Create_Notification()
    {
        // Arrange
        var dto = new CreateTaskItemDto
        {
            Title = "Task",
            CustomerId = 1,
            AssignedUserId = 2
        };

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(new Customer());

        _userManagerMock
            .Setup(x => x.FindByIdAsync("2"))
            .ReturnsAsync(new AppUser { Id = 2 });

        _taskRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<TaskItem, bool>>>()))
            .ReturnsAsync(false);

        // Act
        await _service.CreateAsync(dto);

        // Assert
        _notificationServiceMock.Verify(
            x => x.AddAsync(
                2,
                "New Task Assigned",
                It.IsAny<string>(),
                NotificationType.Info,
                nameof(TaskItem),
                It.IsAny<int>()),
            Times.Once);
    }
    [Fact]
    public async Task CreateAsync_Should_Not_Create_Notification_When_AssignedUser_Is_Null()
    {
        // Arrange
        var dto = new CreateTaskItemDto
        {
            Title = "Task",
            CustomerId = 1,
            AssignedUserId = null
        };

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(new Customer());

        // AssignedUser null olduğu için AnyAsync çağrılmaz.

        // Act
        await _service.CreateAsync(dto);

        // Assert
        _notificationServiceMock.Verify(
            x => x.AddAsync(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<NotificationType>(),
                It.IsAny<string>(),
                It.IsAny<int>()),
            Times.Never);
    }
    [Fact]
    public async Task CreateAsync_Should_Call_DashboardUpdated()
    {
        // Arrange
        var dto = new CreateTaskItemDto
        {
            Title = "Task",
            CustomerId = 1,
            AssignedUserId = 1
        };

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

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
        await _service.CreateAsync(dto);

        // Assert
        _realtimeServiceMock.Verify(
            x => x.DashboardUpdatedAsync(),
            Times.Once);
    }
}