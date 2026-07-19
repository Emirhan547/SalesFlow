using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Business.Services.DealServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Repositories.DealRepositories;
using SalesFlow.DataAccess.Repositories.TagRepositories;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;
using System.Linq.Expressions;

namespace SalesFlow.UnitTests.BusinessRules
{
    public class DealBusinessRulesTests
    {
        private readonly Mock<IDealRepository> _dealRepositoryMock = new();

        private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
        private readonly Mock<ITagRepository> _tagRepositoryMock = new();

        private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();

        private readonly Mock<UserManager<AppUser>> _userManagerMock;

        private readonly DealBusinessRules _businessRules;

        public DealBusinessRulesTests()
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

            var authBusinessRules = new AuthBusinessRules(
                _currentUserServiceMock.Object);

            var customerBusinessRules = new CustomerBusinessRules(
                _customerRepositoryMock.Object,
                _tagRepositoryMock.Object,
                _currentUserServiceMock.Object,
                authBusinessRules);

            _currentUserServiceMock
                .Setup(x => x.UserId)
                .Returns(1);

            _currentUserServiceMock
                .Setup(x => x.IsInRole("Admin"))
                .Returns(true);

            _businessRules = new DealBusinessRules(
                _dealRepositoryMock.Object,
                _userManagerMock.Object,
                customerBusinessRules,
                authBusinessRules);
        }
        [Fact]
        public async Task GetDealByIdAsync_Should_Return_Deal()
        {
            // Arrange
            var deal = new Deal
            {
                Id = 1,
                Title = "CRM Project"
            };

            _dealRepositoryMock
                .Setup(x => x.GetByIdAsync(1, false))
                .ReturnsAsync(deal);

            // Act
            var result = await _businessRules.GetDealByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Title.Should().Be("CRM Project");
        }
        [Fact]
        public async Task GetDealByIdAsync_Should_Throw_When_NotFound()
        {
            // Arrange
            _dealRepositoryMock
                .Setup(x => x.GetByIdAsync(1, false))
                .ReturnsAsync((Deal?)null);

            // Act
            Func<Task> act = () => _businessRules.GetDealByIdAsync(1);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Deal not found.");
        }
        [Fact]
        public async Task EnsureAssignedUserExistsAsync_Should_Not_Throw_When_User_Exists()
        {
            // Arrange
            var user = new AppUser
            {
                Id = 1
            };

            _userManagerMock
                .Setup(x => x.FindByIdAsync("1"))
                .ReturnsAsync(user);

            // Act
            var act = async () =>
                await _businessRules.EnsureAssignedUserExistsAsync(1);

            // Assert
            await act.Should().NotThrowAsync();
        }
        [Fact]
        public async Task EnsureAssignedUserExistsAsync_Should_Throw_When_User_NotFound()
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
        public async Task EnsureAssignedUserExistsAsync_Should_Not_Throw_When_UserId_Is_Null()
        {
            // Act
            var act = async () =>
                await _businessRules.EnsureAssignedUserExistsAsync(null);

            // Assert
            await act.Should().NotThrowAsync();
        }
        [Fact]
        public async Task EnsureActiveDealTitleIsUniqueAsync_Should_Not_Throw()
        {
            // Arrange
            _dealRepositoryMock
                .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Deal, bool>>>()))
                .ReturnsAsync(false);

            // Act
            var act = async () =>
                await _businessRules.EnsureActiveDealTitleIsUniqueAsync(
                    "CRM",
                    1);

            // Assert
            await act.Should().NotThrowAsync();
        }
        [Fact]
        public async Task EnsureActiveDealTitleIsUniqueAsync_Should_Throw()
        {
            // Arrange
            _dealRepositoryMock
                .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Deal, bool>>>()))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = () =>
                _businessRules.EnsureActiveDealTitleIsUniqueAsync(
                    "CRM",
                    1);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessException>()
                .WithMessage("An active deal with the same title already exists for this customer.");
        }
        [Fact]
        public async Task EnsureActiveDealTitleIsUniqueForUpdateAsync_Should_Not_Throw()
        {
            // Arrange
            _dealRepositoryMock
                .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Deal, bool>>>()))
                .ReturnsAsync(false);

            // Act
            var act = async () =>
                await _businessRules.EnsureActiveDealTitleIsUniqueForUpdateAsync(
                    1,
                    "CRM",
                    10);

            // Assert
            await act.Should().NotThrowAsync();
        }
        [Fact]
        public async Task EnsureActiveDealTitleIsUniqueForUpdateAsync_Should_Throw()
        {
            // Arrange
            _dealRepositoryMock
                .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Deal, bool>>>()))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = () =>
                _businessRules.EnsureActiveDealTitleIsUniqueForUpdateAsync(
                    1,
                    "CRM",
                    10);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessException>()
                .WithMessage("An active deal with the same title already exists for this customer.");
        }
        [Theory]
        [InlineData(DealStage.New)]
        [InlineData(DealStage.Qualified)]
        [InlineData(DealStage.ProposalSent)]
        [InlineData(DealStage.Negotiation)]
        public void EnsureDealIsEditable_Should_Not_Throw(
    DealStage stage)
        {
            // Arrange
            var deal = new Deal
            {
                Stage = stage
            };

            // Act
            var act = () =>
                _businessRules.EnsureDealIsEditable(deal);

            // Assert
            act.Should().NotThrow();
        }
        [Theory]
        [InlineData(DealStage.Won)]
        [InlineData(DealStage.Lost)]
        public void EnsureDealIsEditable_Should_Throw_When_Completed(
    DealStage stage)
        {
            // Arrange
            var deal = new Deal
            {
                Stage = stage
            };

            // Act
            var act = () =>
                _businessRules.EnsureDealIsEditable(deal);

            // Assert
            act.Should()
                .Throw<BusinessException>()
                .WithMessage("Completed deals cannot be updated.");
        }
        [Theory]
        [InlineData(DealStage.New)]
        [InlineData(DealStage.Qualified)]
        [InlineData(DealStage.ProposalSent)]
        [InlineData(DealStage.Negotiation)]
        [InlineData(DealStage.Lost)]
        public void EnsureDealIsDeletable_Should_Not_Throw(
    DealStage stage)
        {
            // Arrange
            var deal = new Deal
            {
                Stage = stage
            };

            // Act
            var act = () =>
                _businessRules.EnsureDealIsDeletable(deal);

            // Assert
            act.Should().NotThrow();
        }
        [Fact]
        public void EnsureDealIsDeletable_Should_Throw_When_Won()
        {
            // Arrange
            var deal = new Deal
            {
                Stage = DealStage.Won
            };

            // Act
            var act = () =>
                _businessRules.EnsureDealIsDeletable(deal);

            // Assert
            act.Should()
                .Throw<BusinessException>()
                .WithMessage("Won deals cannot be deleted.");
        }
        [Theory]
        [InlineData(DealStage.New, DealStage.Qualified)]
        [InlineData(DealStage.New, DealStage.Lost)]
        [InlineData(DealStage.Qualified, DealStage.ProposalSent)]
        [InlineData(DealStage.Qualified, DealStage.Lost)]
        [InlineData(DealStage.ProposalSent, DealStage.Negotiation)]
        [InlineData(DealStage.ProposalSent, DealStage.Lost)]
        [InlineData(DealStage.Negotiation, DealStage.Won)]
        [InlineData(DealStage.Negotiation, DealStage.Lost)]
        public void EnsureStageTransition_Should_Allow_Valid_Transitions(
    DealStage current,
    DealStage next)
        {
            // Act
            var act = () =>
                _businessRules.EnsureStageTransition(current, next);

            // Assert
            act.Should().NotThrow();
        }
        [Theory]
        [InlineData(DealStage.New, DealStage.ProposalSent)]
        [InlineData(DealStage.New, DealStage.Negotiation)]
        [InlineData(DealStage.New, DealStage.Won)]

        [InlineData(DealStage.Qualified, DealStage.New)]
        [InlineData(DealStage.Qualified, DealStage.Won)]

        [InlineData(DealStage.ProposalSent, DealStage.New)]
        [InlineData(DealStage.ProposalSent, DealStage.Qualified)]
        [InlineData(DealStage.ProposalSent, DealStage.Won)]

        [InlineData(DealStage.Negotiation, DealStage.New)]
        [InlineData(DealStage.Negotiation, DealStage.Qualified)]
        [InlineData(DealStage.Negotiation, DealStage.ProposalSent)]

        [InlineData(DealStage.Won, DealStage.New)]
        [InlineData(DealStage.Won, DealStage.Qualified)]
        [InlineData(DealStage.Won, DealStage.ProposalSent)]
        [InlineData(DealStage.Won, DealStage.Negotiation)]
        [InlineData(DealStage.Won, DealStage.Lost)]

        [InlineData(DealStage.Lost, DealStage.New)]
        [InlineData(DealStage.Lost, DealStage.Qualified)]
        [InlineData(DealStage.Lost, DealStage.ProposalSent)]
        [InlineData(DealStage.Lost, DealStage.Negotiation)]
        [InlineData(DealStage.Lost, DealStage.Won)]
        public void EnsureStageTransition_Should_Throw_When_Invalid(
    DealStage current,
    DealStage next)
        {
            // Act
            var act = () =>
                _businessRules.EnsureStageTransition(current, next);

            // Assert
            act.Should()
                .Throw<BusinessException>()
                .WithMessage("Invalid deal stage transition.");
        }
        [Fact]
        public void EnsureUserCanModify_Should_Not_Throw()
        {
            // Arrange
            var deal = new Deal
            {
                AssignedUserId = 1
            };

            // Act
            var act = () =>
                _businessRules.EnsureUserCanModify(deal);

            // Assert
            act.Should().NotThrow();
        }
        [Fact]
        public void EnsureUserCanModify_Should_Throw_When_User_Has_No_Access()
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

            var deal = new Deal
            {
                AssignedUserId = 5
            };

            // Act
            var act = () =>
                _businessRules.EnsureUserCanModify(deal);

            // Assert
            act.Should()
                .Throw<ForbiddenException>()
                .WithMessage("You are not authorized to access this record.");
        }
    }
}