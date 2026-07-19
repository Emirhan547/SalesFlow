using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using SalesFlow.Business.Services.ProfileServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.UnitTests.BusinessRules
{
    public class ProfileBusinessRulesTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;

        private readonly ProfileBusinessRules _businessRules;

        public ProfileBusinessRulesTests()
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

            _businessRules = new ProfileBusinessRules(
                _userManagerMock.Object);
        }
        [Fact]
        public async Task GetUserByIdAsync_Should_Return_User()
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

            // Act
            var result = await _businessRules.GetUserByIdAsync(1);

            // Assert
            result.Should().Be(user);
        }
        [Fact]
        public async Task GetUserByIdAsync_Should_Throw_When_User_Not_Found()
        {
            // Arrange
            _userManagerMock
                .Setup(x => x.FindByIdAsync("1"))
                .ReturnsAsync((AppUser?)null);

            // Act
            Func<Task> act =
                () => _businessRules.GetUserByIdAsync(1);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("User not found.");
        }
        [Fact]
        public async Task EnsureUserNameIsAvailableAsync_Should_Not_Throw()
        {
            // Arrange
            _userManagerMock
                .Setup(x => x.FindByNameAsync("emirhan"))
                .ReturnsAsync((AppUser?)null);

            // Act
            await _businessRules.EnsureUserNameIsAvailableAsync(
                "emirhan",
                1);

            // Assert
        }
        [Fact]
        public async Task EnsureUserNameIsAvailableAsync_Should_Not_Throw_When_Same_User()
        {
            // Arrange
            _userManagerMock
                .Setup(x => x.FindByNameAsync("emirhan"))
                .ReturnsAsync(new AppUser
                {
                    Id = 1,
                    UserName = "emirhan"
                });

            // Act
            await _businessRules.EnsureUserNameIsAvailableAsync(
                "emirhan",
                1);

            // Assert
        }
        [Fact]
        public async Task EnsureUserNameIsAvailableAsync_Should_Throw()
        {
            // Arrange
            _userManagerMock
                .Setup(x => x.FindByNameAsync("emirhan"))
                .ReturnsAsync(new AppUser
                {
                    Id = 2,
                    UserName = "emirhan"
                });

            // Act
            Func<Task> act =
                () => _businessRules.EnsureUserNameIsAvailableAsync(
                    "emirhan",
                    1);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessException>()
                .WithMessage("Username is already in use.");
        }
        [Fact]
        public void EnsurePasswordsMatch_Should_Not_Throw()
        {
            // Arrange
            const string password = "123456";

            // Act
            var act = () =>
                _businessRules.EnsurePasswordsMatch(
                    password,
                    password);

            // Assert
            act.Should().NotThrow();
        }
        [Fact]
        public void EnsurePasswordsMatch_Should_Throw()
        {
            // Act
            var act = () =>
                _businessRules.EnsurePasswordsMatch(
                    "123456",
                    "654321");

            // Assert
            act.Should()
                .Throw<BusinessException>()
                .WithMessage("New passwords do not match.");
        }
    }
}
