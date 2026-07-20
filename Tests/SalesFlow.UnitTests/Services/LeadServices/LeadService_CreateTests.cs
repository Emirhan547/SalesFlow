using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using SalesFlow.Business.Dtos.LeadDtos;
using SalesFlow.Business.ML.Services;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Business.Services.AIServices;
using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Business.Services.ExportServices;
using SalesFlow.Business.Services.LeadServices;
using SalesFlow.Business.Services.RealtimeServices;
using SalesFlow.Business.Services.UserServices;
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

public class LeadService_CreateTests
{
    private readonly Mock<ILeadRepository> _leadRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly Mock<IValidator<CreateLeadDto>> _createValidatorMock = new();
    private readonly Mock<IValidator<UpdateLeadDto>> _updateValidatorMock = new();

    private readonly Mock<IActivityLogService> _activityLogServiceMock = new();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
    private readonly Mock<IRealtimeService> _realtimeServiceMock = new();
    private readonly Mock<IValidator<ConvertLeadDto>> _convertValidatorMock = new();

    private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
    private readonly Mock<ITagRepository> _tagRepositoryMock = new();

    private readonly Mock<IDealRepository> _dealRepositoryMock = new();
    private readonly Mock<IMeetingRepository> _meetingRepositoryMock = new();
    private readonly Mock<ITaskItemRepository> _taskRepositoryMock = new();

    private readonly Mock<IExcelExportService> _excelExportServiceMock = new();
    private readonly Mock<IPdfExportService> _pdfExportServiceMock = new();
    private readonly LeadService _service;
    private readonly Mock<IOpenAiService> _openAiServiceMock = new();
    private readonly Mock<ILeadScoringService> _leadScoringServiceMock = new();
    public LeadService_CreateTests()
    {
        var authBusinessRules =
            new AuthBusinessRules(_currentUserServiceMock.Object);

        var leadBusinessRules = new LeadBusinessRules(
     _leadRepositoryMock.Object,
     authBusinessRules);
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
     _pdfExportServiceMock.Object,
     _openAiServiceMock.Object,
     _leadScoringServiceMock.Object);
    }
    [Fact]
    public async Task CreateAsync_Should_Create_Lead_Successfully()
    {
        // Arrange
        var dto = new CreateLeadDto
        {
            FirstName = "Emirhan",
            LastName = "Hacıoğlu",
            Email = "emirhan@test.com",
            CompanyName = "SalesFlow"
        };

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Message.Should().Be("Lead created successfully.");

        _leadRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Lead>()),
            Times.Once);

        _activityLogServiceMock.Verify(
            x => x.AddAsync(
                ActivityAction.Create,
                nameof(Lead),
                It.IsAny<int>(),
                It.IsAny<string>(),
                1),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }
    [Fact]
    public async Task CreateAsync_Should_Map_Dto_To_Lead()
    {
        // Arrange
        var dto = new CreateLeadDto
        {
            FirstName = "Ali",
            LastName = "Yılmaz",
            Email = "ali@test.com",
            CompanyName = "Microsoft"
        };

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        Lead? createdLead = null;

        _leadRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Lead>()))
            .Callback<Lead>(lead => createdLead = lead);

        // Act
        await _service.CreateAsync(dto);

        // Assert
        createdLead.Should().NotBeNull();

        createdLead!.FirstName.Should().Be(dto.FirstName);
        createdLead.LastName.Should().Be(dto.LastName);
        createdLead.Email.Should().Be(dto.Email);
        createdLead.CompanyName.Should().Be(dto.CompanyName);

        createdLead.Status.Should().Be(LeadStatus.New);
    }
}