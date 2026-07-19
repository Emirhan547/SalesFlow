using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using SalesFlow.Business.Dtos.ProfileDtos;
using SalesFlow.Business.Services.ProfileServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.Core.Results;
using SalesFlow.Entity.Entities;

namespace SalesFlow.UnitTests.Services.ProfileServices;

public class ProfileServiceTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;

    private readonly ProfileService _service;

    public ProfileServiceTests()
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

        var businessRules = new ProfileBusinessRules(
            _userManagerMock.Object);

        _service = new ProfileService(
            _userManagerMock.Object,
            businessRules);
    }
    [Fact]
    public async Task GetProfileAsync_Should_Return_Profile()
    {
        // Arrange
        var user = new AppUser
        {
            Id = 1,
            FirstName = "Emirhan",
            LastName = "Hacıoğlu",
            UserName = "emirhan",
            Email = "test@test.com",
            PhoneNumber = "555",
            ProfileImageUrl = "image.png"
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(user);

        // Act
        var result = await _service.GetProfileAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Data.Id.Should().Be(1);
        result.Data.FirstName.Should().Be("Emirhan");
        result.Data.LastName.Should().Be("Hacıoğlu");
        result.Data.UserName.Should().Be("emirhan");
        result.Data.Email.Should().Be("test@test.com");
        result.Data.PhoneNumber.Should().Be("555");
        result.Data.ProfileImageUrl.Should().Be("image.png");
    }
    [Fact]
    public async Task UpdateProfileAsync_Should_Update_Profile()
    {
        // Arrange
        var dto = new UpdateProfileDto
        {
            FirstName = "New",
            LastName = "Name",
            UserName = "newuser",
            PhoneNumber = "111"
        };

        var user = new AppUser
        {
            Id = 1,
            UserName = "olduser"
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.FindByNameAsync("newuser"))
            .ReturnsAsync((AppUser?)null);

        _userManagerMock
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _service.UpdateProfileAsync(1, dto);

        // Assert
        result.IsSuccess.Should().BeTrue();

        user.FirstName.Should().Be(dto.FirstName);
        user.LastName.Should().Be(dto.LastName);
        user.UserName.Should().Be(dto.UserName);
        user.PhoneNumber.Should().Be(dto.PhoneNumber);
    }
    [Fact]
    public async Task UpdateProfileAsync_Should_Return_Failure_When_Update_Fails()
    {
        // Arrange
        var dto = new UpdateProfileDto
        {
            UserName = "newuser"
        };

        var user = new AppUser
        {
            Id = 1
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.FindByNameAsync("newuser"))
            .ReturnsAsync((AppUser?)null);

        _userManagerMock
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(
                IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Update failed."
                    }));

        // Act
        var result = await _service.UpdateProfileAsync(1, dto);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Update failed.");
    }
    [Fact]
    public async Task ChangePasswordAsync_Should_Change_Password()
    {
        // Arrange
        var dto = new ChangePasswordDto
        {
            CurrentPassword = "Old123!",
            NewPassword = "New123!",
            ConfirmNewPassword = "New123!"
        };

        var user = new AppUser
        {
            Id = 1
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.ChangePasswordAsync(
                user,
                dto.CurrentPassword,
                dto.NewPassword))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _service.ChangePasswordAsync(1, dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
    [Fact]
    public async Task ChangePasswordAsync_Should_Return_Failure_When_Identity_Fails()
    {
        // Arrange
        var dto = new ChangePasswordDto
        {
            CurrentPassword = "Old123!",
            NewPassword = "New123!",
            ConfirmNewPassword = "New123!"
        };

        var user = new AppUser
        {
            Id = 1
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.ChangePasswordAsync(
                user,
                dto.CurrentPassword,
                dto.NewPassword))
            .ReturnsAsync(
                IdentityResult.Failed(
                    new IdentityError
                    {
                        Description = "Current password is incorrect."
                    }));

        // Act
        var result = await _service.ChangePasswordAsync(1, dto);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Current password is incorrect.");
    }
    [Fact]
    public async Task ChangePasswordAsync_Should_Throw_When_Passwords_Do_Not_Match()
    {
        // Arrange
        var dto = new ChangePasswordDto
        {
            CurrentPassword = "Old123!",
            NewPassword = "New123!",
            ConfirmNewPassword = "Different123!"
        };

        var user = new AppUser
        {
            Id = 1
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(user);

        // Act
        Func<Task> act = () => _service.ChangePasswordAsync(1, dto);

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("New passwords do not match.");
    }
}