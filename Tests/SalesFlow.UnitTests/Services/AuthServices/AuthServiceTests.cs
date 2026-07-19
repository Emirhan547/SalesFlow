using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using SalesFlow.Business.Dtos.AuthDtos;
using SalesFlow.Business.Dtos.JwtDtos;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Business.Services.JwtServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Constants;
using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;
using MockQueryable.Moq;
namespace SalesFlow.UnitTests.Services.AuthServices;

public class AuthServiceTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<IActivityLogService> _activityLogServiceMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly AuthService _service;

    public AuthServiceTests()
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

        var businessRules = new AuthBusinessRules(
            _currentUserServiceMock.Object);

        _service = new AuthService(
            _userManagerMock.Object,
            businessRules,
            _tokenServiceMock.Object,
            _activityLogServiceMock.Object,
            _unitOfWorkMock.Object);
    }
    [Fact]
    public async Task LoginAsync_Should_Return_Failure_When_User_Not_Found()
    {
        // Arrange
        var dto = new LoginDto
        {
            Email = "test@test.com",
            Password = "123456"
        };

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(dto.Email))
            .ReturnsAsync((AppUser?)null);

        // Act
        var result = await _service.LoginAsync(dto);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Invalid email or password.");
    }
    [Fact]
    public async Task LoginAsync_Should_Throw_When_User_Is_Inactive()
    {
        // Arrange
        var dto = new LoginDto
        {
            Email = "test@test.com",
            Password = "123456"
        };

        var user = new AppUser
        {
            Id = 1,
            Email = dto.Email,
            IsActive = false
        };

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        // Act
        Func<Task> act = () => _service.LoginAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("User account is inactive.");
    }
    [Fact]
    public async Task LoginAsync_Should_Return_Failure_When_Password_Is_Wrong()
    {
        // Arrange
        var dto = new LoginDto
        {
            Email = "test@test.com",
            Password = "wrong"
        };

        var user = new AppUser
        {
            Id = 1,
            Email = dto.Email,
            IsActive = true
        };

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.CheckPasswordAsync(user, dto.Password))
            .ReturnsAsync(false);

        // Act
        var result = await _service.LoginAsync(dto);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Invalid email or password.");
    }
    [Fact]
    public async Task LoginAsync_Should_Login_Successfully()
    {
        // Arrange
        var dto = new LoginDto
        {
            Email = "test@test.com",
            Password = "123456"
        };

        var user = new AppUser
        {
            Id = 1,
            Email = dto.Email,
            UserName = "emirhan",
            IsActive = true
        };

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.CheckPasswordAsync(user, dto.Password))
            .ReturnsAsync(true);

        _userManagerMock
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "Admin" });

        _tokenServiceMock
            .Setup(x => x.CreateAccessToken(It.IsAny<TokenUser>()))
            .Returns("access-token");

        _tokenServiceMock
            .Setup(x => x.CreateRefreshToken())
            .Returns("refresh-token");

        _tokenServiceMock
            .Setup(x => x.GetAccessTokenExpireDate())
            .Returns(DateTime.UtcNow.AddMinutes(30));

        _tokenServiceMock
            .Setup(x => x.GetRefreshTokenExpireDate())
            .Returns(DateTime.UtcNow.AddDays(7));

        _userManagerMock
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _service.LoginAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Data.AccessToken.Should().Be("access-token");
        result.Data.RefreshToken.Should().Be("refresh-token");
    }
    [Fact]
    public async Task LoginAsync_Should_Create_ActivityLog()
    {
        // Arrange
        var dto = new LoginDto
        {
            Email = "test@test.com",
            Password = "123456"
        };

        var user = new AppUser
        {
            Id = 1,
            Email = dto.Email,
            UserName = "emirhan",
            IsActive = true
        };

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.CheckPasswordAsync(user, dto.Password))
            .ReturnsAsync(true);

        _userManagerMock
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(new List<string>());

        _tokenServiceMock
            .Setup(x => x.CreateAccessToken(It.IsAny<TokenUser>()))
            .Returns("token");

        _tokenServiceMock
            .Setup(x => x.CreateRefreshToken())
            .Returns("refresh");

        _tokenServiceMock
            .Setup(x => x.GetAccessTokenExpireDate())
            .Returns(DateTime.UtcNow);

        _tokenServiceMock
            .Setup(x => x.GetRefreshTokenExpireDate())
            .Returns(DateTime.UtcNow.AddDays(7));

        _userManagerMock
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _service.LoginAsync(dto);

        // Assert
        _activityLogServiceMock.Verify(
            x => x.AddAsync(
                ActivityAction.Login,
                nameof(AppUser),
                user.Id,
                $"User '{user.UserName}' logged in.",
                user.Id),
            Times.Once);
    }
    [Fact]
    public async Task LogoutAsync_Should_Return_Failure_When_User_Not_Found()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync((AppUser?)null);

        // Act
        var result = await _service.LogoutAsync(1);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("User not found.");
    }
    [Fact]
    public async Task LogoutAsync_Should_Logout_Successfully()
    {
        // Arrange
        var user = new AppUser
        {
            Id = 1,
            UserName = "emirhan",
            RefreshToken = "refresh-token",
            RefreshTokenExpireDate = DateTime.UtcNow.AddDays(7)
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _service.LogoutAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();

        user.RefreshToken.Should().BeNull();
        user.RefreshTokenExpireDate.Should().BeNull();

        _userManagerMock.Verify(
            x => x.UpdateAsync(user),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }
    [Fact]
    public async Task LogoutAsync_Should_Create_ActivityLog()
    {
        // Arrange
        var user = new AppUser
        {
            Id = 1,
            UserName = "emirhan"
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _service.LogoutAsync(1);

        // Assert
        _activityLogServiceMock.Verify(
            x => x.AddAsync(
                ActivityAction.Logout,
                nameof(AppUser),
                user.Id,
                $"User '{user.UserName}' logged out.",
                user.Id),
            Times.Once);
    }
    [Fact]
    public async Task RegisterAsync_Should_Register_User()
    {
        // Arrange
        var dto = new RegisterDto
        {
            FirstName = "Emirhan",
            LastName = "Hacıoğlu",
            UserName = "emirhan",
            Email = "test@test.com",
            Password = "123456"
        };

        _userManagerMock
            .Setup(x => x.CreateAsync(
                It.IsAny<AppUser>(),
                dto.Password))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock
            .Setup(x => x.AddToRoleAsync(
                It.IsAny<AppUser>(),
                Roles.SalesRepresentative))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _service.RegisterAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _userManagerMock.Verify(
            x => x.CreateAsync(
                It.IsAny<AppUser>(),
                dto.Password),
            Times.Once);

        _userManagerMock.Verify(
            x => x.AddToRoleAsync(
                It.IsAny<AppUser>(),
                Roles.SalesRepresentative),
            Times.Once);
    }
    [Fact]
    public async Task RegisterAsync_Should_Return_Failure_When_Create_Fails()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Password = "123456"
        };

        _userManagerMock
            .Setup(x => x.CreateAsync(
                It.IsAny<AppUser>(),
                dto.Password))
            .ReturnsAsync(
                IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Create failed."
                    }));

        // Act
        var result = await _service.RegisterAsync(dto);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Create failed.");
    }
    [Fact]
    public async Task RegisterAsync_Should_Delete_User_When_AddRole_Fails()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Password = "123456"
        };

        AppUser? createdUser = null;

        _userManagerMock
            .Setup(x => x.CreateAsync(
                It.IsAny<AppUser>(),
                dto.Password))
            .Callback<AppUser, string>((u, _) => createdUser = u)
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock
            .Setup(x => x.AddToRoleAsync(
                It.IsAny<AppUser>(),
                Roles.SalesRepresentative))
            .ReturnsAsync(
                IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Role failed."
                    }));

        _userManagerMock
            .Setup(x => x.DeleteAsync(It.IsAny<AppUser>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _service.RegisterAsync(dto);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Role failed.");

        _userManagerMock.Verify(
            x => x.DeleteAsync(createdUser!),
            Times.Once);
    }
    
    }
