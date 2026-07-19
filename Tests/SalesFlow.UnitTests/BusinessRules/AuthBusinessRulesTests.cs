using FluentAssertions;
using Moq;
using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.Entity.Entities;
using Xunit;

namespace SalesFlow.UnitTests.BusinessRules;

public class AuthBusinessRulesTests
{
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly AuthBusinessRules _businessRules;

    public AuthBusinessRulesTests()
    {
        _currentUserServiceMock = new Mock<ICurrentUserService>();

        _businessRules = new AuthBusinessRules(
            _currentUserServiceMock.Object);
    }

    [Fact]
    public void EnsureUserIsActive_Should_NotThrow_When_UserIsActive()
    {
        // Arrange
        var user = new AppUser
        {
            IsActive = true
        };

        // Act
        Action act = () =>
            _businessRules.EnsureUserIsActive(user);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void EnsureUserIsActive_Should_ThrowBusinessException_When_UserIsInactive()
    {
        // Arrange
        var user = new AppUser
        {
            IsActive = false
        };

        // Act
        Action act = () =>
            _businessRules.EnsureUserIsActive(user);

        // Assert
        act.Should()
            .Throw<BusinessException>()
            .WithMessage("User account is inactive.");
    }

    [Fact]
    public void EnsureCurrentUserCanAccess_Should_NotThrow_When_UserIsAdmin()
    {
        // Arrange
        _currentUserServiceMock
            .Setup(x => x.IsInRole("Admin"))
            .Returns(true);

        // Act
        Action act = () =>
            _businessRules.EnsureCurrentUserCanAccess(5);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void EnsureCurrentUserCanAccess_Should_NotThrow_When_UserIsSalesManager()
    {
        // Arrange
        _currentUserServiceMock
            .Setup(x => x.IsInRole("Admin"))
            .Returns(false);

        _currentUserServiceMock
            .Setup(x => x.IsInRole("SalesManager"))
            .Returns(true);

        // Act
        Action act = () =>
            _businessRules.EnsureCurrentUserCanAccess(5);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void EnsureCurrentUserCanAccess_Should_ThrowForbiddenException_When_UserIdIsNull()
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
            .Returns((int?)null);

        // Act
        Action act = () =>
            _businessRules.EnsureCurrentUserCanAccess(5);

        // Assert
        act.Should()
            .Throw<ForbiddenException>()
            .WithMessage("User information could not be determined.");
    }

    [Fact]
    public void EnsureCurrentUserCanAccess_Should_ThrowForbiddenException_When_UserTriesToAccessAnotherUsersResource()
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

        // Act
        Action act = () =>
            _businessRules.EnsureCurrentUserCanAccess(2);

        // Assert
        act.Should()
            .Throw<ForbiddenException>()
            .WithMessage("You are not authorized to access this resource.");
    }

    [Fact]
    public void EnsureCurrentUserCanAccess_Should_NotThrow_When_UserAccessesOwnResource()
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

        // Act
        Action act = () =>
            _businessRules.EnsureCurrentUserCanAccess(5);

        // Assert
        act.Should().NotThrow();
    }
}