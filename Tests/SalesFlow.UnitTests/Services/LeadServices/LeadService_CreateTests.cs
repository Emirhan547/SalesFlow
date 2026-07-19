using FluentAssertions;
using FluentValidation;
using Moq;
using SalesFlow.Business.Dtos.LeadDtos;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Business.Services.LeadServices;
using SalesFlow.Business.Services.RealtimeServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.DataAccess.Repositories.LeadRepositories;
using SalesFlow.DataAccess.Uows;

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

    private readonly LeadService _service;

    public LeadService_CreateTests()
    {
        var authBusinessRules =
            new AuthBusinessRules(_currentUserServiceMock.Object);

        var leadBusinessRules =
            new LeadBusinessRules(
                _leadRepositoryMock.Object,
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
            _activityLogServiceMock.Object,
            _currentUserServiceMock.Object,
            _realtimeServiceMock.Object);
    }
}