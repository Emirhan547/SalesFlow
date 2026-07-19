using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Business.Services.LeadServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.LeadRepositories;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;
using Xunit;

namespace SalesFlow.UnitTests.BusinessRules;

public class LeadBusinessRulesTests
{
    private readonly Mock<ILeadRepository> _leadRepositoryMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;

    private readonly LeadBusinessRules _businessRules;

    public LeadBusinessRulesTests()
    {
        _leadRepositoryMock = new Mock<ILeadRepository>();

        _currentUserServiceMock = new Mock<ICurrentUserService>();

        var authBusinessRules =
            new AuthBusinessRules(_currentUserServiceMock.Object);

        _businessRules =
            new LeadBusinessRules(
                _leadRepositoryMock.Object,
                authBusinessRules);
    }

    #region EnsureEmailIsUniqueAsync

    [Fact]
    public async Task EnsureEmailIsUniqueAsync_Should_NotThrow_When_EmailIsUnique()
    {
        _leadRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Lead, bool>>>()))
            .ReturnsAsync(false);

        Func<Task> act = () =>
            _businessRules.EnsureEmailIsUniqueAsync("test@test.com");

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task EnsureEmailIsUniqueAsync_Should_ThrowBusinessException_When_EmailAlreadyExists()
    {
        _leadRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Lead, bool>>>()))
            .ReturnsAsync(true);

        Func<Task> act = () =>
            _businessRules.EnsureEmailIsUniqueAsync("test@test.com");

        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("A lead with this email already exists.");
    }

    #endregion

    #region EnsureEmailIsUniqueForUpdateAsync

    [Fact]
    public async Task EnsureEmailIsUniqueForUpdateAsync_Should_NotThrow_When_EmailIsUnique()
    {
        _leadRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Lead, bool>>>()))
            .ReturnsAsync(false);

        Func<Task> act = () =>
            _businessRules.EnsureEmailIsUniqueForUpdateAsync(1, "test@test.com");

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task EnsureEmailIsUniqueForUpdateAsync_Should_ThrowBusinessException_When_EmailAlreadyExists()
    {
        _leadRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Lead, bool>>>()))
            .ReturnsAsync(true);

        Func<Task> act = () =>
            _businessRules.EnsureEmailIsUniqueForUpdateAsync(1, "test@test.com");

        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("A lead with this email already exists.");
    }

    #endregion

    #region GetLeadByIdAsync

    [Fact]
    public async Task GetLeadByIdAsync_Should_ReturnLead_When_LeadExists()
    {
        var lead = new Lead { Id = 1 };

        _leadRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(lead);

        var result = await _businessRules.GetLeadByIdAsync(1);

        result.Should().Be(lead);
    }

    [Fact]
    public async Task GetLeadByIdAsync_Should_ThrowNotFoundException_When_LeadDoesNotExist()
    {
        _leadRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync((Lead?)null);

        Func<Task> act = () =>
            _businessRules.GetLeadByIdAsync(1);

        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("Lead not found.");
    }

    #endregion

    #region EnsureLeadCanBeConverted

    [Fact]
    public void EnsureLeadCanBeConverted_Should_NotThrow_When_LeadIsQualified()
    {
        var lead = new Lead
        {
            Status = LeadStatus.Qualified
        };

        Action act = () =>
            _businessRules.EnsureLeadCanBeConverted(lead);

        act.Should().NotThrow();
    }

    [Fact]
    public void EnsureLeadCanBeConverted_Should_ThrowBusinessException_When_LeadIsConverted()
    {
        var lead = new Lead
        {
            Status = LeadStatus.Converted
        };

        Action act = () =>
            _businessRules.EnsureLeadCanBeConverted(lead);

        act.Should()
            .Throw<BusinessException>()
            .WithMessage("Lead has already been converted.");
    }

    [Fact]
    public void EnsureLeadCanBeConverted_Should_ThrowBusinessException_When_LeadIsNotQualified()
    {
        var lead = new Lead
        {
            Status = LeadStatus.New
        };

        Action act = () =>
            _businessRules.EnsureLeadCanBeConverted(lead);

        act.Should()
            .Throw<BusinessException>()
            .WithMessage("Only qualified leads can be converted.");
    }

    #endregion

    #region EnsureUserCanModify

    [Fact]
    public void EnsureUserCanModify_Should_NotThrow_When_CurrentUserOwnsLead()
    {
        _currentUserServiceMock.Setup(x => x.IsInRole("Admin")).Returns(false);
        _currentUserServiceMock.Setup(x => x.IsInRole("SalesManager")).Returns(false);
        _currentUserServiceMock.Setup(x => x.UserId).Returns(5);

        var lead = new Lead
        {
            AssignedUserId = 5
        };

        Action act = () =>
            _businessRules.EnsureUserCanModify(lead);

        act.Should().NotThrow();
    }

    [Fact]
    public void EnsureUserCanModify_Should_NotThrow_When_CurrentUserIsAdmin()
    {
        _currentUserServiceMock
            .Setup(x => x.IsInRole("Admin"))
            .Returns(true);

        var lead = new Lead
        {
            AssignedUserId = 999
        };

        Action act = () =>
            _businessRules.EnsureUserCanModify(lead);

        act.Should().NotThrow();
    }

    [Fact]
    public void EnsureUserCanModify_Should_ThrowForbiddenException_When_CurrentUserDoesNotOwnLead()
    {
        _currentUserServiceMock.Setup(x => x.IsInRole("Admin")).Returns(false);
        _currentUserServiceMock.Setup(x => x.IsInRole("SalesManager")).Returns(false);
        _currentUserServiceMock.Setup(x => x.UserId).Returns(1);

        var lead = new Lead
        {
            AssignedUserId = 2
        };

        Action act = () =>
            _businessRules.EnsureUserCanModify(lead);

        act.Should()
            .Throw<ForbiddenException>()
            .WithMessage("You are not authorized to access this resource.");
    }

    #endregion
}