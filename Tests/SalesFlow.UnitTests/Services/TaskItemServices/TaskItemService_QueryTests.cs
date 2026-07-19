using FluentAssertions;
using FluentValidation;
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
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Repositories.TagRepositories;
using SalesFlow.DataAccess.Repositories.TaskItemRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;

namespace SalesFlow.UnitTests.Services.TaskItemServices;

public class TaskItemService_QueryTests
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

    public TaskItemService_QueryTests()
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
}