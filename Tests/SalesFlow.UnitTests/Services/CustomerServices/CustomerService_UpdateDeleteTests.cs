using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using SalesFlow.Business.Dtos.CustomerDtos;
using SalesFlow.Business.Dtos.JwtDtos;
using SalesFlow.Business.Dtos.UserDtos;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Business.Services.AIServices;
using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Business.Services.ExportServices;
using SalesFlow.Business.Services.RealtimeServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Repositories.TagRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;
using System.Linq.Expressions;

namespace SalesFlow.UnitTests.Services.CustomerServices;

public class CustomerService_UpdateDeleteTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly Mock<IValidator<CreateCustomerDto>> _createValidatorMock = new();
    private readonly Mock<IValidator<UpdateCustomerDto>> _updateValidatorMock = new();

    private readonly Mock<IActivityLogService> _activityLogServiceMock = new();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();

    private readonly Mock<IExcelExportService> _excelExportServiceMock = new();
    private readonly Mock<IPdfExportService> _pdfExportServiceMock = new();

    private readonly Mock<IRealtimeService> _realtimeServiceMock = new();

    private readonly Mock<ITagRepository> _tagRepositoryMock = new();
    private readonly Mock<IOpenAiService> _openAiServiceMock = new();

    private readonly CustomerService _service;

    public CustomerService_UpdateDeleteTests()
    {
        var authBusinessRules =
            new AuthBusinessRules(_currentUserServiceMock.Object);

        var customerBusinessRules =
            new CustomerBusinessRules(
                _customerRepositoryMock.Object,
                _tagRepositoryMock.Object,
                _currentUserServiceMock.Object,
                authBusinessRules);

        _currentUserServiceMock
            .Setup(x => x.UserId)
            .Returns(1);

        _currentUserServiceMock
      .Setup(x => x.UserId)
      .Returns(1);

        _currentUserServiceMock
            .Setup(x => x.IsInRole("Admin"))
            .Returns(true);

        _currentUserServiceMock
            .Setup(x => x.IsInRole("Admin"))
            .Returns(true);

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(
                It.IsAny<ValidationContext<UpdateCustomerDto>>(),
                default))
            .ReturnsAsync(new ValidationResult());

        _service = new CustomerService(
            _customerRepositoryMock.Object,
            _unitOfWorkMock.Object,
            customerBusinessRules,
            _createValidatorMock.Object,
            _updateValidatorMock.Object,
            _activityLogServiceMock.Object,
            _currentUserServiceMock.Object,
            _excelExportServiceMock.Object,
            _pdfExportServiceMock.Object,
            _realtimeServiceMock.Object,
             _openAiServiceMock.Object);
    }

    [Fact]
    public async Task UpdateAsync_Should_UpdateCustomerSuccessfully()
    {
        // Arrange
        var dto = new UpdateCustomerDto
        {
            Id = 1,
            ContactFirstName = "Updated",
            ContactLastName = "Customer",
            Email = "updated@test.com"
        };

        var customer = new Customer
        {
            Id = 1,
            ContactFirstName = "Old",
            ContactLastName = "Name",
            Email = "old@test.com",
            AssignedUserId = 1
        };

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(dto.Id, true))
            .ReturnsAsync(customer);

        _customerRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(false);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _service.UpdateAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be("Customer updated successfully.");

        customer.ContactFirstName.Should().Be(dto.ContactFirstName);
        customer.ContactLastName.Should().Be(dto.ContactLastName);
        customer.Email.Should().Be(dto.Email);

        _customerRepositoryMock.Verify(
            x => x.Update(customer),
            Times.Once);

        _activityLogServiceMock.Verify(
            x => x.AddAsync(
                ActivityAction.Update,
                nameof(Customer),
                customer.Id,
                It.IsAny<string>(),
                1),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);

        _realtimeServiceMock.Verify(
            x => x.DashboardUpdatedAsync(),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Customer_NotFound()
    {
        // Arrange
        var dto = new UpdateCustomerDto
        {
            Id = 1,
            Email = "test@test.com"
        };

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(dto.Id, true))
            .ReturnsAsync((Customer?)null);

        // Act
        Func<Task> act = () => _service.UpdateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("Customer not found.");

        _customerRepositoryMock.Verify(
            x => x.Update(It.IsAny<Customer>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Email_AlreadyExists()
    {
        // Arrange
        var dto = new UpdateCustomerDto
        {
            Id = 1,
            Email = "existing@test.com"
        };

        var customer = new Customer
        {
            Id = 1,
            Email = "old@test.com",
            AssignedUserId = 1
        };

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(dto.Id, true))
            .ReturnsAsync(customer);

        _customerRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(true);

        // Act
        Func<Task> act = () => _service.UpdateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>();

        _customerRepositoryMock.Verify(
            x => x.Update(It.IsAny<Customer>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_Should_DeleteCustomerSuccessfully()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1,
            ContactFirstName = "Ali",
            ContactLastName = "Veli",
            AssignedUserId = 1
        };

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(customer);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _customerRepositoryMock.Verify(
            x => x.Delete(customer),
            Times.Once);

        _activityLogServiceMock.Verify(
            x => x.AddAsync(
                ActivityAction.Delete,
                nameof(Customer),
                customer.Id,
                It.IsAny<string>(),
                1),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);

        _realtimeServiceMock.Verify(
            x => x.DashboardUpdatedAsync(),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Should_Throw_When_Customer_NotFound()
    {
        // Arrange
        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync((Customer?)null);

        // Act
        Func<Task> act = () => _service.DeleteAsync(1);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>();

        _customerRepositoryMock.Verify(
            x => x.Delete(It.IsAny<Customer>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }
}