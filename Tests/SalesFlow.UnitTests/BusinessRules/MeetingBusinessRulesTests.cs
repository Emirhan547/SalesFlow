using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Business.Services.MeetingServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Repositories.MeetingRepositories;
using SalesFlow.DataAccess.Repositories.TagRepositories;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;
using System.Linq.Expressions;

namespace SalesFlow.UnitTests.BusinessRules;

public class MeetingBusinessRulesTests
{
    private readonly Mock<IMeetingRepository> _meetingRepositoryMock = new();

    private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();

    private readonly Mock<ITagRepository> _tagRepositoryMock = new();

    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();

    private readonly Mock<UserManager<AppUser>> _userManagerMock;

    private readonly MeetingBusinessRules _businessRules;

    public MeetingBusinessRulesTests()
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
            new MeetingBusinessRules(
                _meetingRepositoryMock.Object,
                _userManagerMock.Object,
                customerBusinessRules,
                authBusinessRules);
    }
    [Fact]
    public async Task GetMeetingByIdAsync_Should_Return_Meeting()
    {
        // Arrange
        var meeting = new Meeting
        {
            Id = 1,
            Title = "Sprint Planning"
        };

        _meetingRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(meeting);

        // Act
        var result = await _businessRules.GetMeetingByIdAsync(1);

        // Assert
        result.Should().Be(meeting);
    }
    [Fact]
    public async Task GetMeetingByIdAsync_Should_Throw_When_NotFound()
    {
        // Arrange
        _meetingRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync((Meeting?)null);

        // Act
        Func<Task> act = () =>
            _businessRules.GetMeetingByIdAsync(1);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("Meeting not found.");
    }
    [Fact]
    public async Task EnsureAssignedUserExistsAsync_Should_Not_Throw()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(new AppUser { Id = 1 });

        // Act
        var act = async () =>
            await _businessRules.EnsureAssignedUserExistsAsync(1);

        // Assert
        await act.Should().NotThrowAsync();
    }
    [Fact]
    public async Task EnsureAssignedUserExistsAsync_Should_Not_Throw_When_UserId_Is_Null()
    {
        // Act
        var act = async () =>
            await _businessRules.EnsureAssignedUserExistsAsync(null);

        // Assert
        await act.Should().NotThrowAsync();
    }
    [Fact]
    public async Task EnsureAssignedUserExistsAsync_Should_Throw()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync((AppUser?)null);

        // Act
        Func<Task> act = () =>
            _businessRules.EnsureAssignedUserExistsAsync(1);

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("Assigned user not found.");
    }
    [Fact]
    public async Task HasMeetingConflictAsync_Should_Return_False_When_User_Is_Null()
    {
        // Act
        var result = await _businessRules.HasMeetingConflictAsync(
            null,
            DateTime.Now,
            DateTime.Now.AddHours(1));

        // Assert
        result.Should().BeFalse();
    }
    [Fact]
    public async Task EnsureNoMeetingConflictAsync_Should_Not_Throw()
    {
        // Arrange
        var meetings = new List<Meeting>()
            .AsQueryable();

        _meetingRepositoryMock
            .Setup(x => x.GetAll(false))
            .Returns(meetings);

        // Act
        var act = async () =>
            await _businessRules.EnsureNoMeetingConflictAsync(
                1,
                new DateTime(2026, 1, 1, 10, 0, 0),
                new DateTime(2026, 1, 1, 11, 0, 0));

        // Assert
        await act.Should().NotThrowAsync();
    }
    [Fact]
    public async Task EnsureNoMeetingConflictAsync_Should_Throw()
    {
        // Arrange
        var meetings = new List<Meeting>
    {
        new()
        {
            Id = 1,
            AssignedUserId = 1,
            Status = MeetingStatus.Scheduled,
            StartDate = new DateTime(2026,1,1,10,0,0),
            EndDate = new DateTime(2026,1,1,11,0,0)
        }
    }.AsQueryable();

        _meetingRepositoryMock
            .Setup(x => x.GetAll(false))
            .Returns(meetings);

        // Act
        Func<Task> act = () =>
            _businessRules.EnsureNoMeetingConflictAsync(
                1,
                new DateTime(2026, 1, 1, 10, 30, 0),
                new DateTime(2026, 1, 1, 11, 30, 0));

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("The assigned user already has another meeting during this time.");
    }
    [Fact]
    public async Task EnsureNoMeetingConflictForUpdateAsync_Should_Not_Throw()
    {
        // Arrange
        var meetings = new List<Meeting>
    {
        new()
        {
            Id = 1,
            AssignedUserId = 1,
            Status = MeetingStatus.Scheduled,
            StartDate = new DateTime(2026,1,1,10,0,0),
            EndDate = new DateTime(2026,1,1,11,0,0)
        }
    }.AsQueryable();

        _meetingRepositoryMock
            .Setup(x => x.GetAll(false))
            .Returns(meetings);

        // Act
        var act = async () =>
            await _businessRules.EnsureNoMeetingConflictForUpdateAsync(
                1,
                1,
                new DateTime(2026, 1, 1, 10, 30, 0),
                new DateTime(2026, 1, 1, 11, 30, 0));

        // Assert
        await act.Should().NotThrowAsync();
    }
    [Fact]
    public async Task EnsureNoMeetingConflictForUpdateAsync_Should_Throw()
    {
        // Arrange
        var meetings = new List<Meeting>
    {
        new()
        {
            Id = 2,
            AssignedUserId = 1,
            Status = MeetingStatus.Scheduled,
            StartDate = new DateTime(2026,1,1,10,0,0),
            EndDate = new DateTime(2026,1,1,11,0,0)
        }
    }.AsQueryable();

        _meetingRepositoryMock
            .Setup(x => x.GetAll(false))
            .Returns(meetings);

        // Act
        Func<Task> act = () =>
            _businessRules.EnsureNoMeetingConflictForUpdateAsync(
                1,
                1,
                new DateTime(2026, 1, 1, 10, 30, 0),
                new DateTime(2026, 1, 1, 11, 30, 0));

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("The assigned user already has another meeting during this time.");
    }
    [Fact]
    public void EnsureMeetingIsEditable_Should_Not_Throw()
    {
        // Arrange
        var meeting = new Meeting
        {
            Status = MeetingStatus.Scheduled
        };

        // Act
        var act = () =>
            _businessRules.EnsureMeetingIsEditable(meeting);

        // Assert
        act.Should().NotThrow();
    }
    [Fact]
    public void EnsureMeetingIsEditable_Should_Throw_When_Completed()
    {
        // Arrange
        var meeting = new Meeting
        {
            Status = MeetingStatus.Completed
        };

        // Act
        var act = () =>
            _businessRules.EnsureMeetingIsEditable(meeting);

        // Assert
        act.Should()
            .Throw<BusinessException>()
            .WithMessage("Completed or cancelled meetings cannot be updated.");
    }
    [Fact]
    public void EnsureMeetingIsEditable_Should_Throw_When_Cancelled()
    {
        // Arrange
        var meeting = new Meeting
        {
            Status = MeetingStatus.Cancelled
        };

        // Act
        var act = () =>
            _businessRules.EnsureMeetingIsEditable(meeting);

        // Assert
        act.Should()
            .Throw<BusinessException>()
            .WithMessage("Completed or cancelled meetings cannot be updated.");
    }
    [Theory]
    [InlineData(MeetingStatus.Scheduled, MeetingStatus.Completed)]
    [InlineData(MeetingStatus.Scheduled, MeetingStatus.Cancelled)]
    public void EnsureStatusTransition_Should_Allow_Valid(
    MeetingStatus current,
    MeetingStatus next)
    {
        // Act
        var act = () =>
            _businessRules.EnsureStatusTransition(
                current,
                next);

        // Assert
        act.Should().NotThrow();
    }
    [Theory]
    [InlineData(MeetingStatus.Scheduled, MeetingStatus.Scheduled)]

    [InlineData(MeetingStatus.Completed, MeetingStatus.Scheduled)]
    [InlineData(MeetingStatus.Completed, MeetingStatus.Completed)]
    [InlineData(MeetingStatus.Completed, MeetingStatus.Cancelled)]

    [InlineData(MeetingStatus.Cancelled, MeetingStatus.Scheduled)]
    [InlineData(MeetingStatus.Cancelled, MeetingStatus.Completed)]
    [InlineData(MeetingStatus.Cancelled, MeetingStatus.Cancelled)]
    public void EnsureStatusTransition_Should_Throw_When_Invalid(
    MeetingStatus current,
    MeetingStatus next)
    {
        // Act
        var act = () =>
            _businessRules.EnsureStatusTransition(
                current,
                next);

        // Assert
        act.Should()
            .Throw<BusinessException>()
            .WithMessage("Invalid meeting status transition.");
    }
    [Fact]
    public void EnsureUserCanModify_Should_Not_Throw()
    {
        // Arrange
        var meeting = new Meeting
        {
            AssignedUserId = 1
        };

        // Act
        var act = () =>
            _businessRules.EnsureUserCanModify(meeting);

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

        var meeting = new Meeting
        {
            AssignedUserId = 5
        };

        // Act
        var act = () =>
            _businessRules.EnsureUserCanModify(meeting);

        // Assert
        act.Should()
            .Throw<ForbiddenException>()
            .WithMessage("You are not authorized to access this record.");
    }
}