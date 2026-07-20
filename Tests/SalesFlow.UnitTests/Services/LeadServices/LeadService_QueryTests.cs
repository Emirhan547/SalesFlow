using FluentAssertions;
using FluentValidation;
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

namespace SalesFlow.UnitTests.Services.LeadServices;

public class LeadService_QueryTests
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
    private readonly Mock<ILeadScoringService> _leadScoringServiceMock = new();
    private readonly LeadService _service;
    private readonly Mock<IOpenAiService> _openAiServiceMock = new();
    public LeadService_QueryTests()
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
            _pdfExportServiceMock.Object, _openAiServiceMock.Object, _leadScoringServiceMock.Object);
    }
    [Fact]
    public async Task GetByIdAsync_Should_Return_Lead()
    {
        // Arrange
        var lead = new Lead
        {
            Id = 1,
            FirstName = "Emirhan",
            LastName = "Hacıoğlu",
            Email = "emirhan@test.com",
            CompanyName = "SalesFlow",
            AssignedUserId = 1
        };

        _leadRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(lead);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Data.Should().NotBeNull();
        result.Data.Id.Should().Be(1);
        result.Data.FirstName.Should().Be("Emirhan");
        result.Data.LastName.Should().Be("Hacıoğlu");
        result.Data.Email.Should().Be("emirhan@test.com");
    }
    [Fact]
    public async Task GetByIdAsync_Should_Throw_When_User_Has_No_Access()
    {
        // Arrange
        var lead = new Lead
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

        _leadRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(lead);

        // Act
        Func<Task> act = () => _service.GetByIdAsync(1);

        // Assert
        await act.Should()
            .ThrowAsync<ForbiddenException>()
            .WithMessage("You are not authorized to access this record.");
    }
}