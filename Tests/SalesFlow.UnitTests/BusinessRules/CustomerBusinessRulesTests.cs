using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Repositories.TagRepositories;
using SalesFlow.Entity.Entities;
using Xunit;

namespace SalesFlow.UnitTests.BusinessRules;

public class CustomerBusinessRulesTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;

    private readonly CustomerBusinessRules _businessRules;

    public CustomerBusinessRulesTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();

        _tagRepositoryMock = new Mock<ITagRepository>();

        _currentUserServiceMock = new Mock<ICurrentUserService>();

        var authBusinessRules =
            new AuthBusinessRules(_currentUserServiceMock.Object);

        _businessRules =
            new CustomerBusinessRules(
                _customerRepositoryMock.Object,
                _tagRepositoryMock.Object,
                _currentUserServiceMock.Object,
                authBusinessRules);
    }

    #region EnsureEmailIsUniqueAsync

    [Fact]
    public async Task EnsureEmailIsUniqueAsync_Should_NotThrow_When_EmailIsUnique()
    {
        _customerRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(false);

        Func<Task> act = () =>
            _businessRules.EnsureEmailIsUniqueAsync("test@test.com");

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task EnsureEmailIsUniqueAsync_Should_ThrowBusinessException_When_EmailAlreadyExists()
    {
        _customerRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(true);

        Func<Task> act = () =>
            _businessRules.EnsureEmailIsUniqueAsync("test@test.com");

        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("A customer with this email already exists.");
    }

    #endregion

    #region EnsureEmailIsUniqueForUpdateAsync

    [Fact]
    public async Task EnsureEmailIsUniqueForUpdateAsync_Should_NotThrow_When_EmailIsUnique()
    {
        _customerRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(false);

        Func<Task> act = () =>
            _businessRules.EnsureEmailIsUniqueForUpdateAsync(1, "test@test.com");

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task EnsureEmailIsUniqueForUpdateAsync_Should_ThrowBusinessException_When_EmailAlreadyExists()
    {
        _customerRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(true);

        Func<Task> act = () =>
            _businessRules.EnsureEmailIsUniqueForUpdateAsync(1, "test@test.com");

        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("A customer with this email already exists.");
    }

    #endregion

    #region EnsureCustomerExistsAsync

    [Fact]
    public async Task EnsureCustomerExistsAsync_Should_NotThrow_When_CustomerExists()
    {
        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(new Customer { Id = 1 });

        Func<Task> act = () =>
            _businessRules.EnsureCustomerExistsAsync(1);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task EnsureCustomerExistsAsync_Should_ThrowNotFoundException_When_CustomerDoesNotExist()
    {
        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync((Customer?)null);

        Func<Task> act = () =>
            _businessRules.EnsureCustomerExistsAsync(1);

        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("Customer not found.");
    }
    #endregion

    #region GetCustomerByIdAsync

    [Fact]
    public async Task GetCustomerByIdAsync_Should_ReturnCustomer_When_CustomerExists()
    {
        var customer = new Customer { Id = 1 };

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(customer);

        var result = await _businessRules.GetCustomerByIdAsync(1);

        result.Should().Be(customer);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_Should_ThrowNotFoundException_When_CustomerDoesNotExist()
    {
        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync((Customer?)null);

        Func<Task> act = () =>
            _businessRules.GetCustomerByIdAsync(1);

        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("Customer not found.");
    }

    #endregion
    #region EnsureTagExistsAsync

    [Fact]
    public async Task EnsureTagExistsAsync_Should_NotThrow_When_TagExists()
    {
        _tagRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Tag, bool>>>()))
            .ReturnsAsync(true);

        Func<Task> act = () =>
            _businessRules.EnsureTagExistsAsync(1);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task EnsureTagExistsAsync_Should_ThrowBusinessException_When_TagDoesNotExist()
    {
        _tagRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Tag, bool>>>()))
            .ReturnsAsync(false);

        Func<Task> act = () =>
            _businessRules.EnsureTagExistsAsync(1);

        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("Tag not found.");
    }

    #endregion

    #region EnsureCustomerTagNotExistsAsync

    [Fact]
    public async Task EnsureCustomerTagNotExistsAsync_Should_NotThrow_When_TagIsNotAssigned()
    {
        _customerRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(false);

        Func<Task> act = () =>
            _businessRules.EnsureCustomerTagNotExistsAsync(1, 1);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task EnsureCustomerTagNotExistsAsync_Should_ThrowBusinessException_When_TagAlreadyAssigned()
    {
        _customerRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(true);

        Func<Task> act = () =>
            _businessRules.EnsureCustomerTagNotExistsAsync(1, 1);

        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("This customer already has this tag.");
    }

    #endregion

    #region GetCustomerTagAsync

    [Fact]
    public async Task GetCustomerTagAsync_Should_ThrowNotFoundException_When_CustomerDoesNotExist()
    {
        _customerRepositoryMock
            .Setup(x => x.GetCustomerWithTagsAsync(1, true))
            .ReturnsAsync((Customer?)null);

        Func<Task> act = () =>
            _businessRules.GetCustomerTagAsync(1, 1);

        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("Customer not found.");
    }

    [Fact]
    public async Task GetCustomerTagAsync_Should_ThrowBusinessException_When_CustomerTagDoesNotExist()
    {
        var customer = new Customer
        {
            CustomerTags = new List<CustomerTag>()
        };

        _customerRepositoryMock
            .Setup(x => x.GetCustomerWithTagsAsync(1, true))
            .ReturnsAsync(customer);

        Func<Task> act = () =>
            _businessRules.GetCustomerTagAsync(1, 1);

        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("Customer tag not found.");
    }

    [Fact]
    public async Task GetCustomerTagAsync_Should_ReturnCustomerTag_When_CustomerTagExists()
    {
        var customerTag = new CustomerTag
        {
            CustomerId = 1,
            TagId = 5
        };

        var customer = new Customer
        {
            CustomerTags = new List<CustomerTag>
        {
            customerTag
        }
        };

        _customerRepositoryMock
            .Setup(x => x.GetCustomerWithTagsAsync(1, true))
            .ReturnsAsync(customer);

        var result = await _businessRules.GetCustomerTagAsync(1, 5);

        result.Should().Be(customerTag);
    }

    #endregion

    #region EnsureUserCanAccess

    [Fact]
    public void EnsureUserCanAccess_Should_NotThrow_When_CurrentUserOwnsCustomer()
    {
        _currentUserServiceMock
            .Setup(x => x.IsInRole("Admin"))
            .Returns(false);

        _currentUserServiceMock
            .Setup(x => x.IsInRole("SalesManager"))
            .Returns(false);

        _currentUserServiceMock
            .Setup(x => x.UserId)
            .Returns(5);

        var customer = new Customer
        {
            AssignedUserId = 5
        };

        Action act = () =>
            _businessRules.EnsureUserCanAccess(customer);

        act.Should().NotThrow();
    }

    [Fact]
    public void EnsureUserCanAccess_Should_NotThrow_When_CurrentUserIsAdmin()
    {
        _currentUserServiceMock
            .Setup(x => x.IsInRole("Admin"))
            .Returns(true);

        var customer = new Customer
        {
            AssignedUserId = 999
        };

        Action act = () =>
            _businessRules.EnsureUserCanAccess(customer);

        act.Should().NotThrow();
    }

    [Fact]
    public void EnsureUserCanAccess_Should_ThrowForbiddenException_When_CurrentUserDoesNotOwnCustomer()
    {
        _currentUserServiceMock
            .Setup(x => x.IsInRole("Admin"))
            .Returns(false);

        _currentUserServiceMock
            .Setup(x => x.IsInRole("SalesManager"))
            .Returns(false);

        _currentUserServiceMock
            .Setup(x => x.UserId)
            .Returns(1);

        var customer = new Customer
        {
            AssignedUserId = 2
        };

        Action act = () =>
            _businessRules.EnsureUserCanAccess(customer);

        act.Should()
            .Throw<ForbiddenException>()
            .WithMessage("You are not authorized to access this resource.");
    }

    #endregion
}