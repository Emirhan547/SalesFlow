using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
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

public class MeetingService_CreateTests
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

    public MeetingService_CreateTests()
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
    public async Task CreateAsync_Should_Create_Meeting_Successfully()
    {
        // Arrange
        var dto = new CreateMeetingDto
        {
            Title = "Sprint Planning",
            CustomerId = 1,
            AssignedUserId = 1,
            StartDate = new DateTime(2026, 1, 1, 10, 0, 0),
            EndDate = new DateTime(2026, 1, 1, 11, 0, 0)
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

        _meetingRepositoryMock
            .Setup(x => x.GetAll(false))
            .Returns(new List<Meeting>().AsQueryable());

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _meetingRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Meeting>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);

        _activityLogServiceMock.Verify(
            x => x.AddAsync(
                ActivityAction.Create,
                nameof(Meeting),
                It.IsAny<int>(),
                It.IsAny<string>(),
                1),
            Times.Once);

        _notificationServiceMock.Verify(
            x => x.AddAsync(
                1,
                "New Meeting Scheduled",
                It.IsAny<string>(),
                NotificationType.Reminder,
                nameof(Meeting),
                It.IsAny<int>()),
            Times.Once);

        _realtimeServiceMock.Verify(
            x => x.DashboardUpdatedAsync(),
            Times.Once);
    }
    [Fact]
    public async Task CreateAsync_Should_Map_Dto_To_Meeting()
    {
        // Arrange
        var dto = new CreateMeetingDto
        {
            Title = "Demo",
            Description = "Description",
            CustomerId = 1,
            AssignedUserId = 1,
            Location = "Meeting Room",
            StartDate = new DateTime(2026, 1, 1, 10, 0, 0),
            EndDate = new DateTime(2026, 1, 1, 11, 0, 0)
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

        _meetingRepositoryMock
            .Setup(x => x.GetAll(false))
            .Returns(new List<Meeting>().AsQueryable());

        Meeting? createdMeeting = null;

        _meetingRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Meeting>()))
            .Callback<Meeting>(x => createdMeeting = x);

        // Act
        await _service.CreateAsync(dto);

        // Assert
        createdMeeting.Should().NotBeNull();

        createdMeeting!.Title.Should().Be(dto.Title);
        createdMeeting.Description.Should().Be(dto.Description);
        createdMeeting.CustomerId.Should().Be(dto.CustomerId);
        createdMeeting.AssignedUserId.Should().Be(dto.AssignedUserId);
        createdMeeting.Location.Should().Be(dto.Location);

        createdMeeting.Status.Should().Be(MeetingStatus.Scheduled);
    }
    [Fact]
    public async Task CreateAsync_Should_Throw_When_Validation_Fails()
    {
        // Arrange
        var dto = new CreateMeetingDto();

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
    public async Task CreateAsync_Should_Throw_When_Assigned_User_Not_Found()
    {
        // Arrange
        var dto = new CreateMeetingDto
        {
            Title = "Meeting",
            CustomerId = 1,
            AssignedUserId = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddHours(1)
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
    public async Task CreateAsync_Should_Throw_When_Customer_Not_Found()
    {
        // Arrange
        var dto = new CreateMeetingDto
        {
            Title = "Meeting",
            CustomerId = 1,
            AssignedUserId = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddHours(1)
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
    public async Task CreateAsync_Should_Throw_When_Meeting_Conflict_Exists()
    {
        // Arrange
        var dto = new CreateMeetingDto
        {
            Title = "Meeting",
            CustomerId = 1,
            AssignedUserId = 1,
            StartDate = new DateTime(2026, 1, 1, 10, 30, 0),
            EndDate = new DateTime(2026, 1, 1, 11, 30, 0)
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
        Func<Task> act = () => _service.CreateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("The assigned user already has another meeting during this time.");
    }
    [Fact]
    public async Task CreateAsync_Should_Not_Create_Notification_When_AssignedUser_Is_Null()
    {
        // Arrange
        var dto = new CreateMeetingDto
        {
            Title = "Meeting",
            CustomerId = 1,
            AssignedUserId = null,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddHours(1)
        };

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(new Customer { Id = 1 });

        _meetingRepositoryMock
            .Setup(x => x.GetAll(false))
            .Returns(new List<Meeting>().AsQueryable());

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
        var dto = new CreateMeetingDto
        {
            Title = "Meeting",
            CustomerId = 1,
            AssignedUserId = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddHours(1)
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

        _meetingRepositoryMock
            .Setup(x => x.GetAll(false))
            .Returns(new List<Meeting>().AsQueryable());

        // Act
        await _service.CreateAsync(dto);

        // Assert
        _realtimeServiceMock.Verify(
            x => x.DashboardUpdatedAsync(),
            Times.Once);
    }
}