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

public class MeetingService_UpdateDeleteTests
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

    public MeetingService_UpdateDeleteTests()
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
    public async Task UpdateAsync_Should_Update_Meeting_Successfully()
    {
        // Arrange
        var dto = new UpdateMeetingDto
        {
            Id = 1,
            Title = "Updated Meeting",
            CustomerId = 1,
            AssignedUserId = 1,
            Status = MeetingStatus.Completed,
            StartDate = new DateTime(2026, 1, 1, 10, 0, 0),
            EndDate = new DateTime(2026, 1, 1, 11, 0, 0)
        };

        var meeting = new Meeting
        {
            Id = 1,
            Title = "Old Meeting",
            CustomerId = 1,
            AssignedUserId = 1,
            Status = MeetingStatus.Scheduled,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate
        };

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _meetingRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(meeting);

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
        var result = await _service.UpdateAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _meetingRepositoryMock.Verify(
            x => x.Update(meeting),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);

        _activityLogServiceMock.Verify(
            x => x.AddAsync(
                ActivityAction.Update,
                nameof(Meeting),
                meeting.Id,
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
        // Arrange
        var dto = new UpdateMeetingDto
        {
            Id = 1,
            Title = "Demo Updated",
            Description = "Updated Description",
            CustomerId = 1,
            AssignedUserId = 1,
            Status = MeetingStatus.Completed,
            Location = "Room B",
            StartDate = new DateTime(2026, 1, 1, 10, 0, 0),
            EndDate = new DateTime(2026, 1, 1, 11, 0, 0)
        };

        var meeting = new Meeting
        {
            Id = 1,
            Status = MeetingStatus.Scheduled
        };

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _meetingRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(meeting);

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
        await _service.UpdateAsync(dto);

        // Assert
        meeting.Title.Should().Be(dto.Title);
        meeting.Description.Should().Be(dto.Description);
        meeting.Location.Should().Be(dto.Location);
        meeting.Status.Should().Be(dto.Status);
        meeting.CustomerId.Should().Be(dto.CustomerId);
        meeting.AssignedUserId.Should().Be(dto.AssignedUserId);
    }
    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Validation_Fails()
    {
        // Arrange
        var dto = new UpdateMeetingDto();

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult(new[]
            {
            new ValidationFailure("Title", "Required")
            }));

        // Act
        Func<Task> act = () => _service.UpdateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<ValidationException>();
    }
    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Meeting_Not_Found()
    {
        // Arrange
        var dto = new UpdateMeetingDto
        {
            Id = 1
        };

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _meetingRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync((Meeting?)null);

        // Act
        Func<Task> act = () => _service.UpdateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("Meeting not found.");
    }
    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Meeting_Is_Not_Editable()
    {
        // Arrange
        var dto = new UpdateMeetingDto
        {
            Id = 1,
            CustomerId = 1,
            AssignedUserId = 1,
            Status = MeetingStatus.Completed,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddHours(1)
        };

        var meeting = new Meeting
        {
            Id = 1,
            CustomerId = 1,
            AssignedUserId = 1,
            Status = MeetingStatus.Completed
        };

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _meetingRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(meeting);

        // Act
        Func<Task> act = () => _service.UpdateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("Completed or cancelled meetings cannot be updated.");
    }
    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Status_Transition_Is_Invalid()
    {
        // Arrange
        var dto = new UpdateMeetingDto
        {
            Id = 1,
            CustomerId = 1,
            AssignedUserId = 1,
            Status = MeetingStatus.Scheduled,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddHours(1)
        };

        var meeting = new Meeting
        {
            Id = 1,
            CustomerId = 1,
            AssignedUserId = 1,
            Status = MeetingStatus.Completed
        };

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _meetingRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(meeting);

        // Act
        Func<Task> act = () => _service.UpdateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("Completed or cancelled meetings cannot be updated.");
    }
    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Assigned_User_Has_Conflict()
    {
        // Arrange
        var dto = new UpdateMeetingDto
        {
            Id = 1,
            CustomerId = 1,
            AssignedUserId = 1,
            Status = MeetingStatus.Scheduled,
            StartDate = new DateTime(2026, 1, 1, 10, 30, 0),
            EndDate = new DateTime(2026, 1, 1, 11, 30, 0)
        };

        var meeting = new Meeting
        {
            Id = 1,
            CustomerId = 1,
            AssignedUserId = 1,
            Status = MeetingStatus.Scheduled
        };

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _meetingRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(meeting);

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
                Id = 2,
                AssignedUserId = 1,
                Status = MeetingStatus.Scheduled,
                StartDate = new DateTime(2026,1,1,10,0,0),
                EndDate = new DateTime(2026,1,1,11,0,0)
            }
            }.AsQueryable());

        // Act
        Func<Task> act = () => _service.UpdateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("The assigned user already has another meeting during this time.");
    }
    [Fact]
    public async Task UpdateAsync_Should_Create_Assignment_Notification()
    {
        // Arrange
        var dto = new UpdateMeetingDto
        {
            Id = 1,
            Title = "Meeting",
            CustomerId = 1,
            AssignedUserId = 2,
            Status = MeetingStatus.Completed,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddHours(1)
        };

        var meeting = new Meeting
        {
            Id = 1,
            Title = "Meeting",
            CustomerId = 1,
            AssignedUserId = 1,
            Status = MeetingStatus.Scheduled,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate
        };

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _meetingRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(meeting);

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(new Customer { Id = 1 });

        _userManagerMock
            .Setup(x => x.FindByIdAsync("2"))
            .ReturnsAsync(new AppUser { Id = 2 });

        _meetingRepositoryMock
            .Setup(x => x.GetAll(false))
            .Returns(new List<Meeting>().AsQueryable());

        // Act
        await _service.UpdateAsync(dto);

        // Assert
        _notificationServiceMock.Verify(
            x => x.AddAsync(
                2,
                "Meeting Assigned",
                It.IsAny<string>(),
                NotificationType.Reminder,
                nameof(Meeting),
                It.IsAny<int>()),
            Times.Once);
    }
    [Fact]
    public async Task DeleteAsync_Should_Delete_Meeting_Successfully()
    {
        // Arrange
        var meeting = new Meeting
        {
            Id = 1,
            Title = "Demo",
            AssignedUserId = 1
        };

        _meetingRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(meeting);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _meetingRepositoryMock.Verify(
            x => x.Delete(meeting),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);

        _activityLogServiceMock.Verify(
            x => x.AddAsync(
                ActivityAction.Delete,
                nameof(Meeting),
                meeting.Id,
                It.IsAny<string>(),
                1),
            Times.Once);

        _realtimeServiceMock.Verify(
            x => x.DashboardUpdatedAsync(),
            Times.Once);
    }
    [Fact]
    public async Task DeleteAsync_Should_Throw_When_Meeting_Not_Found()
    {
        // Arrange
        _meetingRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync((Meeting?)null);

        // Act
        Func<Task> act = () => _service.DeleteAsync(1);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("Meeting not found.");
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

        var meeting = new Meeting
        {
            Id = 1,
            AssignedUserId = 5
        };

        _meetingRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(meeting);

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
        var meeting = new Meeting
        {
            Id = 1,
            Title = "Demo",
            AssignedUserId = 1
        };

        _meetingRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(meeting);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _activityLogServiceMock.Verify(
            x => x.AddAsync(
                ActivityAction.Delete,
                nameof(Meeting),
                meeting.Id,
                It.IsAny<string>(),
                1),
            Times.Once);
    }
    [Fact]
    public async Task DeleteAsync_Should_Call_DashboardUpdated()
    {
        // Arrange
        var meeting = new Meeting
        {
            Id = 1,
            AssignedUserId = 1
        };

        _meetingRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(meeting);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _realtimeServiceMock.Verify(
            x => x.DashboardUpdatedAsync(),
            Times.Once);
    }
}