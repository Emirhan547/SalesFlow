using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Moq;
using SalesFlow.Business.Dtos.MeetingDtos;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Business.Services.MeetingServices;
using SalesFlow.Business.Services.NotificationServices;
using SalesFlow.Business.Services.RealtimeServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Repositories.MeetingRepositories;
using SalesFlow.DataAccess.Repositories.TagRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;

namespace SalesFlow.UnitTests.Services.MeetingServices;

public class MeetingService_QueryTests
{
    private readonly Mock<IMeetingRepository> _meetingRepositoryMock = new();
    private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
    private readonly Mock<ITagRepository> _tagRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly Mock<IValidator<CreateMeetingDto>> _createValidatorMock = new();
    private readonly Mock<IValidator<UpdateMeetingDto>> _updateValidatorMock = new();

    private readonly Mock<IActivityLogService> _activityLogServiceMock = new();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
    private readonly Mock<IRealtimeService> _realtimeServiceMock = new();
    private readonly Mock<INotificationService> _notificationServiceMock = new();

    private readonly Mock<UserManager<AppUser>> _userManagerMock;

    private readonly MeetingService _service;

    public MeetingService_QueryTests()
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

        var meetingBusinessRules = new MeetingBusinessRules(
            _meetingRepositoryMock.Object,
            _userManagerMock.Object,
            customerBusinessRules,
            authBusinessRules);

        _service = new MeetingService(
            _meetingRepositoryMock.Object,
            _unitOfWorkMock.Object,
            meetingBusinessRules,
            _createValidatorMock.Object,
            _updateValidatorMock.Object,
            _activityLogServiceMock.Object,
            _currentUserServiceMock.Object,
            _realtimeServiceMock.Object,
            _notificationServiceMock.Object);
    }
    [Fact]
    public async Task CheckAvailabilityAsync_Should_Return_True_When_Available()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(new AppUser { Id = 1 });

        _meetingRepositoryMock
            .Setup(x => x.GetAll(false))
            .Returns(new List<Meeting>().AsQueryable());

        // Act
        var result = await _service.CheckAvailabilityAsync(
            1,
            new DateTime(2026, 1, 1, 10, 0, 0),
            new DateTime(2026, 1, 1, 11, 0, 0));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeTrue();
    }
    [Fact]
    public async Task CheckAvailabilityAsync_Should_Return_False_When_Conflict_Exists()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(new AppUser { Id = 1 });

        _meetingRepositoryMock
            .Setup(x => x.GetAll(false))
            .Returns(new List<Meeting>
            {
            new()
            {
                Id = 1,
                AssignedUserId = 1,
                Status = MeetingStatus.Scheduled,
                StartDate = new DateTime(2026,1,1,10,0,0),
                EndDate = new DateTime(2026,1,1,11,0,0)
            }
            }.AsQueryable());

        // Act
        var result = await _service.CheckAvailabilityAsync(
            1,
            new DateTime(2026, 1, 1, 10, 30, 0),
            new DateTime(2026, 1, 1, 11, 30, 0));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeFalse();
    }
    [Fact]
    public async Task CheckAvailabilityAsync_Should_Return_Failure_When_EndDate_Is_Invalid()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(new AppUser { Id = 1 });

        // Act
        var result = await _service.CheckAvailabilityAsync(
            1,
            new DateTime(2026, 1, 1, 11, 0, 0),
            new DateTime(2026, 1, 1, 10, 0, 0));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("End date must be later than start date.");
    }
    [Fact]
    public async Task CheckAvailabilityAsync_Should_Throw_When_User_Not_Found()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync((AppUser?)null);

        // Act
        Func<Task> act = () =>
            _service.CheckAvailabilityAsync(
                1,
                DateTime.Now,
                DateTime.Now.AddHours(1));

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("Assigned user not found.");
    }
    [Fact]
    public async Task CheckAvailabilityAsync_Should_Throw_When_User_Has_No_Access()
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
            .Returns(5);

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(new AppUser { Id = 1 });

        // Act
        Func<Task> act = () =>
            _service.CheckAvailabilityAsync(
                1,
                DateTime.Now,
                DateTime.Now.AddHours(1));

        // Assert
        await act.Should()
            .ThrowAsync<ForbiddenException>()
            .WithMessage("You are not authorized to access this record.");
    }
}