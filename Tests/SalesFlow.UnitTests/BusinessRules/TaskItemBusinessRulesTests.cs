using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Business.Services.TaskItemServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Repositories.TagRepositories;
using SalesFlow.DataAccess.Repositories.TaskItemRepositories;
using SalesFlow.Entity.Entities;
using System.Linq.Expressions;
using TaskStatus = SalesFlow.Entity.Enums.TaskStatus;

namespace SalesFlow.UnitTests.BusinessRules;

public class TaskItemBusinessRulesTests
{
    private readonly Mock<ITaskItemRepository> _taskRepositoryMock = new();
    private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
    private readonly Mock<ITagRepository> _tagRepositoryMock = new();

    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();

    private readonly Mock<UserManager<AppUser>> _userManagerMock;

    private readonly TaskItemBusinessRules _businessRules;

    public TaskItemBusinessRulesTests()
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

        _businessRules =
            new TaskItemBusinessRules(
                _taskRepositoryMock.Object,
                _userManagerMock.Object,
                customerBusinessRules,
                authBusinessRules);
    }
    [Fact]
    public async Task GetTaskItemByIdAsync_Should_Return_Task()
    {
        // Arrange
        var task = new TaskItem
        {
            Id = 1
        };

        _taskRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(task);

        // Act
        var result = await _businessRules.GetTaskItemByIdAsync(1);

        // Assert
        result.Should().Be(task);
    }
    [Fact]
    public async Task GetTaskItemByIdAsync_Should_Throw_When_NotFound()
    {
        // Arrange
        _taskRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync((TaskItem?)null);

        // Act
        Func<Task> act =
            () => _businessRules.GetTaskItemByIdAsync(1);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("Task not found.");
    }
    [Fact]
    public async Task EnsureAssignedUserExistsAsync_Should_Not_Throw()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(new AppUser { Id = 1 });

        await _businessRules
            .EnsureAssignedUserExistsAsync(1);
    }
    [Fact]
    public async Task EnsureAssignedUserExistsAsync_Should_Not_Throw_When_UserId_Is_Null()
    {
        await _businessRules
            .EnsureAssignedUserExistsAsync(null);
    }
    [Fact]
    public async Task EnsureAssignedUserExistsAsync_Should_Throw()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync((AppUser?)null);

        Func<Task> act =
            () => _businessRules.EnsureAssignedUserExistsAsync(1);

        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("Assigned user not found.");
    }
    [Fact]
    public async Task EnsureActiveTaskTitleIsUniqueAsync_Should_Not_Throw()
    {
        _taskRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<TaskItem, bool>>>()))
            .ReturnsAsync(false);

        await _businessRules
            .EnsureActiveTaskTitleIsUniqueAsync(
                "Task",
                1);
    }
    [Fact]
    public async Task EnsureActiveTaskTitleIsUniqueAsync_Should_Not_Throw_When_User_Is_Null()
    {
        await _businessRules
            .EnsureActiveTaskTitleIsUniqueAsync(
                "Task",
                null);
    }
    [Fact]
    public async Task EnsureActiveTaskTitleIsUniqueAsync_Should_Throw()
    {
        _taskRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<TaskItem, bool>>>()))
            .ReturnsAsync(true);

        Func<Task> act =
            () => _businessRules
                .EnsureActiveTaskTitleIsUniqueAsync(
                    "Task",
                    1);

        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("An active task with the same title already exists for this user.");
    }
    [Fact]
    public async Task EnsureActiveTaskTitleIsUniqueForUpdateAsync_Should_Not_Throw()
    {
        // Arrange
        _taskRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<TaskItem, bool>>>()))
            .ReturnsAsync(false);

        // Act
        await _businessRules
            .EnsureActiveTaskTitleIsUniqueForUpdateAsync(
                1,
                "Task",
                1);

        // Assert
    }
    [Fact]
    public async Task EnsureActiveTaskTitleIsUniqueForUpdateAsync_Should_Not_Throw_When_User_Is_Null()
    {
        // Act
        await _businessRules
            .EnsureActiveTaskTitleIsUniqueForUpdateAsync(
                1,
                "Task",
                null);

        // Assert
    }
    [Fact]
    public async Task EnsureActiveTaskTitleIsUniqueForUpdateAsync_Should_Throw()
    {
        // Arrange
        _taskRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<TaskItem, bool>>>()))
            .ReturnsAsync(true);

        // Act
        Func<Task> act = () =>
            _businessRules
                .EnsureActiveTaskTitleIsUniqueForUpdateAsync(
                    1,
                    "Task",
                    1);

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("An active task with the same title already exists for this user.");
    }
    [Fact]
    public void EnsureTaskIsEditable_Should_Not_Throw()
    {
        // Arrange
        var task = new TaskItem
        {
            Status = TaskStatus.Pending
        };

        // Act
        var act = () => _businessRules.EnsureTaskIsEditable(task);

        // Assert
        act.Should().NotThrow();
    }
    [Fact]
    public void EnsureTaskIsEditable_Should_Throw_When_Completed()
    {
        // Arrange
        var task = new TaskItem
        {
            Status = TaskStatus.Completed
        };

        // Act
        var act = () => _businessRules.EnsureTaskIsEditable(task);

        // Assert
        act.Should()
            .Throw<BusinessException>()
            .WithMessage("Completed or cancelled tasks cannot be updated.");
    }
    [Fact]
    public void EnsureTaskIsEditable_Should_Throw_When_Cancelled()
    {
        // Arrange
        var task = new TaskItem
        {
            Status = TaskStatus.Cancelled
        };

        // Act
        var act = () => _businessRules.EnsureTaskIsEditable(task);

        // Assert
        act.Should()
            .Throw<BusinessException>()
            .WithMessage("Completed or cancelled tasks cannot be updated.");
    }
    [Theory]
    [InlineData(TaskStatus.Pending, TaskStatus.InProgress)]
    [InlineData(TaskStatus.Pending, TaskStatus.Cancelled)]
    [InlineData(TaskStatus.InProgress, TaskStatus.Completed)]
    [InlineData(TaskStatus.InProgress, TaskStatus.Cancelled)]
    public void EnsureStatusTransition_Should_Allow_Valid(
    TaskStatus current,
    TaskStatus next)
    {
        // Act
        var act = () =>
            _businessRules.EnsureStatusTransition(current, next);

        // Assert
        act.Should().NotThrow();
    }
    [Theory]
    [InlineData(TaskStatus.Pending, TaskStatus.Completed)]
    [InlineData(TaskStatus.InProgress, TaskStatus.Pending)]
    [InlineData(TaskStatus.Completed, TaskStatus.Pending)]
    [InlineData(TaskStatus.Completed, TaskStatus.InProgress)]
    [InlineData(TaskStatus.Cancelled, TaskStatus.Pending)]
    [InlineData(TaskStatus.Cancelled, TaskStatus.InProgress)]
    public void EnsureStatusTransition_Should_Throw_When_Invalid(
    TaskStatus current,
    TaskStatus next)
    {
        // Act
        var act = () =>
            _businessRules.EnsureStatusTransition(current, next);

        // Assert
        act.Should()
            .Throw<BusinessException>()
            .WithMessage("Invalid task status transition.");
    }
    [Fact]
    public void EnsureUserCanModify_Should_Not_Throw()
    {
        // Arrange
        var task = new TaskItem
        {
            AssignedUserId = 1
        };

        // Act
        var act = () => _businessRules.EnsureUserCanModify(task);

        // Assert
        act.Should().NotThrow();
    }
    [Fact]
    public void EnsureUserCanModify_Should_Throw()
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
            AssignedUserId = 5
        };

        // Act
        var act = () => _businessRules.EnsureUserCanModify(task);

        // Assert
        act.Should()
            .Throw<ForbiddenException>()
            .WithMessage("You are not authorized to access this record.");
    }
}