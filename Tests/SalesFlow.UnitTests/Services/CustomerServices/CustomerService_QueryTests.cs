using FluentAssertions;
using FluentValidation;
using Moq;
using SalesFlow.Business.Dtos.CustomerDtos;
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

namespace SalesFlow.UnitTests.Services.CustomerServices;

public class CustomerService_QueryTests
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

    public CustomerService_QueryTests()
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
            .Setup(x => x.IsInRole("Admin"))
            .Returns(true);

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
            _realtimeServiceMock.Object, _openAiServiceMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Customer()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1,
            ContactFirstName = "Emirhan",
            ContactLastName = "Hacıoğlu",
            Email = "emirhan@test.com",
            AssignedUserId = 1
        };

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(customer);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Id.Should().Be(1);
        result.Data.ContactFirstName.Should().Be("Emirhan");
        result.Data.ContactLastName.Should().Be("Hacıoğlu");
        result.Data.Email.Should().Be("emirhan@test.com");
    }

    [Fact]
    public async Task GetByIdAsync_Should_Throw_When_User_Has_No_Access()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1,
            AssignedUserId = 5
        };

        _currentUserServiceMock
            .Setup(x => x.IsInRole("Admin"))
            .Returns(false);

        _currentUserServiceMock
            .Setup(x => x.IsInRole("SalesManager"))
            .Returns(false);

        _currentUserServiceMock
            .Setup(x => x.UserId)
            .Returns(1);

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(customer);

        // Act
        Func<Task> act = () => _service.GetByIdAsync(1);

        // Assert
        await act.Should()
            .ThrowAsync<ForbiddenException>()
            .WithMessage("You are not authorized to access this record.");
    }
}