using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using SalesFlow.Business.Dtos.CustomerDtos;
using SalesFlow.Business.Dtos.LeadDtos;
using SalesFlow.Business.ML.Services;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Business.Services.AIServices;
using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Business.Services.ExportServices;
using SalesFlow.Business.Services.LeadServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Repositories.DealRepositories;
using SalesFlow.DataAccess.Repositories.LeadRepositories;
using SalesFlow.DataAccess.Repositories.MeetingRepositories;
using SalesFlow.DataAccess.Repositories.TagRepositories;
using SalesFlow.DataAccess.Repositories.TaskItemRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;
using System.Linq.Expressions;

namespace SalesFlow.UnitTests.Services.LeadServices;

public class LeadService_ConvertTests
{
    private readonly Mock<ILeadRepository> _leadRepositoryMock = new();
    private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
    private readonly Mock<IDealRepository> _dealRepositoryMock = new();
    private readonly Mock<IMeetingRepository> _meetingRepositoryMock = new();
    private readonly Mock<ITaskItemRepository> _taskRepositoryMock = new();
    private readonly Mock<ITagRepository> _tagRepositoryMock = new();

    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly Mock<IValidator<CreateLeadDto>> _createValidatorMock = new();
    private readonly Mock<IValidator<UpdateLeadDto>> _updateValidatorMock = new();
    private readonly Mock<IValidator<ConvertLeadDto>> _convertValidatorMock = new();

    private readonly Mock<IValidator<CreateCustomerDto>> _createCustomerValidatorMock = new();
    private readonly Mock<IValidator<UpdateCustomerDto>> _updateCustomerValidatorMock = new();

    private readonly Mock<IActivityLogService> _activityLogServiceMock = new();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();

    private readonly Mock<IExcelExportService> _excelExportServiceMock = new();
    private readonly Mock<IPdfExportService> _pdfExportServiceMock = new();
    private readonly Mock<IOpenAiService> _openAiServiceMock = new();
    private readonly Mock<ILeadScoringService> _leadScoringServiceMock = new();
    private readonly LeadService _service;

    public LeadService_ConvertTests()
    {
        var authBusinessRules = new AuthBusinessRules(
            _currentUserServiceMock.Object);

        var customerBusinessRules = new CustomerBusinessRules(
            _customerRepositoryMock.Object,
            _tagRepositoryMock.Object,
            _currentUserServiceMock.Object,
            authBusinessRules);

        var leadBusinessRules = new LeadBusinessRules(
            _leadRepositoryMock.Object,
            authBusinessRules);

        _currentUserServiceMock
            .Setup(x => x.UserId)
            .Returns(1);

        _currentUserServiceMock
            .Setup(x => x.IsInRole("Admin"))
            .Returns(true);

        _service = new LeadService(
            _leadRepositoryMock.Object,
            _unitOfWorkMock.Object,
            leadBusinessRules,
            _createValidatorMock.Object,
            _updateValidatorMock.Object,
            _convertValidatorMock.Object,
            _customerRepositoryMock.Object,
            customerBusinessRules,
            _dealRepositoryMock.Object,
            _meetingRepositoryMock.Object,
            _taskRepositoryMock.Object,
            _activityLogServiceMock.Object,
            _currentUserServiceMock.Object,
            _excelExportServiceMock.Object,
            _pdfExportServiceMock.Object
            , _openAiServiceMock.Object, _leadScoringServiceMock.Object);
    }
    [Fact]
    public async Task ConvertAsync_Should_Create_Initial_Meeting()
    {
        // Arrange
        var dto = new ConvertLeadDto
        {
            CustomerType = CustomerType.Company,
            CreateInitialMeeting = true
        };

        var lead = new Lead
        {
            Id = 1,
            FirstName = "Emirhan",
            LastName = "Hacıoğlu",
            Email = "emirhan@test.com",
            Status = LeadStatus.Qualified,
            AssignedUserId = 1
        };

        _convertValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _leadRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(lead);

        _customerRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(false);

        // Act
        await _service.ConvertAsync(1, dto);

        // Assert
        _meetingRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Meeting>()),
            Times.Once);
    }
    [Fact]
    public async Task ConvertAsync_Should_Create_Initial_Task()
    {
        // Arrange
        var dto = new ConvertLeadDto
        {
            CustomerType = CustomerType.Company,
            CreateInitialTask = true
        };

        var lead = new Lead
        {
            Id = 1,
            FirstName = "Emirhan",
            LastName = "Hacıoğlu",
            Email = "emirhan@test.com",
            Status = LeadStatus.Qualified,
            AssignedUserId = 1
        };

        _convertValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _leadRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(lead);

        _customerRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(false);

        // Act
        await _service.ConvertAsync(1, dto);

        // Assert
        _taskRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<TaskItem>()),
            Times.Once);
    }
    [Fact]
    public async Task ConvertAsync_Should_Throw_When_Lead_Is_Not_Qualified()
    {
        // Arrange
        var dto = new ConvertLeadDto
        {
            CustomerType = CustomerType.Company
        };

        var lead = new Lead
        {
            Id = 1,
            Status = LeadStatus.New,
            AssignedUserId = 1
        };

        _convertValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _leadRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(lead);

        // Act
        Func<Task> act = () => _service.ConvertAsync(1, dto);

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("Only qualified leads can be converted.");
    }
    [Fact]
    public async Task ConvertAsync_Should_Throw_When_Lead_Already_Converted()
    {
        // Arrange
        var dto = new ConvertLeadDto
        {
            CustomerType = CustomerType.Company
        };

        var lead = new Lead
        {
            Id = 1,
            Status = LeadStatus.Converted,
            AssignedUserId = 1
        };

        _convertValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _leadRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(lead);

        // Act
        Func<Task> act = () => _service.ConvertAsync(1, dto);

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("Lead has already been converted.");
    }
    [Fact]
    public async Task ConvertAsync_Should_Throw_When_Customer_Email_Already_Exists()
    {
        // Arrange
        var dto = new ConvertLeadDto
        {
            CustomerType = CustomerType.Company
        };

        var lead = new Lead
        {
            Id = 1,
            Email = "emirhan@test.com",
            Status = LeadStatus.Qualified,
            AssignedUserId = 1
        };

        _convertValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _leadRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(lead);

        _customerRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(true);

        // Act
        Func<Task> act = () => _service.ConvertAsync(1, dto);

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("A customer with this email already exists.");
    }
}