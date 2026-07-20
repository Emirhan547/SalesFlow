using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using SalesFlow.Business.Dtos.CustomerDtos;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Business.Services.AIServices;
using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Business.Services.ExportServices;
using SalesFlow.Business.Services.RealtimeServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Results;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Repositories.TagRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;
using System.Linq.Expressions;

namespace SalesFlow.UnitTests.Services.CustomerServices;

public class CustomerService_CreateTests
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

    private readonly CustomerService _service;
    private readonly Mock<IOpenAiService> _openAiServiceMock = new();
    public CustomerService_CreateTests()
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

        _createValidatorMock
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<CreateCustomerDto>>(), default))
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
    public async Task CreateAsync_Should_CreateCustomerSuccessfully()
    {
        // Arrange
        var dto = new CreateCustomerDto
        {
            ContactFirstName = "Emirhan",
            ContactLastName = "Hacıoğlu",
            Email = "emirhan@test.com"
        };

        _customerRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(false);

        _customerRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Customer>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        Result result = await _service.CreateAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be("Customer created successfully.");

        _customerRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Customer>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);

        _activityLogServiceMock.Verify(
            x => x.AddAsync(
                ActivityAction.Create,
                nameof(Customer),
                It.IsAny<int>(),
                It.IsAny<string>(),
                1),
            Times.Once);

        _realtimeServiceMock.Verify(
            x => x.DashboardUpdatedAsync(),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_Should_MapDto_ToCustomer()
    {
        // Arrange
        Customer? createdCustomer = null;

        var dto = new CreateCustomerDto
        {
            ContactFirstName = "Ali",
            ContactLastName = "Veli",
            Email = "ali@test.com"
        };

        _customerRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(false);

        _customerRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Customer>()))
            .Callback<Customer>(x => createdCustomer = x)
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        await _service.CreateAsync(dto);

        // Assert
        createdCustomer.Should().NotBeNull();

        createdCustomer!.ContactFirstName.Should().Be(dto.ContactFirstName);
        createdCustomer.ContactLastName.Should().Be(dto.ContactLastName);
        createdCustomer.Email.Should().Be(dto.Email);
    }

    

   
}