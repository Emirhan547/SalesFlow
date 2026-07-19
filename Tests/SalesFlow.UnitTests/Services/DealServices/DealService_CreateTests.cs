using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Moq;
using SalesFlow.Business.Dtos.CustomerDtos;
using SalesFlow.Business.Dtos.DealDtos;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Business.Services.DealServices;
using SalesFlow.Business.Services.ExportServices;
using SalesFlow.Business.Services.RealtimeServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Repositories.DealRepositories;
using SalesFlow.DataAccess.Repositories.TagRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;
using System.Linq.Expressions;

namespace SalesFlow.UnitTests.Services.DealServices;

public class DealService_CreateTests
{
    private readonly Mock<IDealRepository> _dealRepositoryMock = new();

    private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();

    private readonly Mock<ITagRepository> _tagRepositoryMock = new();

    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly Mock<IValidator<CreateDealDto>> _createValidatorMock = new();

    private readonly Mock<IValidator<UpdateDealDto>> _updateValidatorMock = new();

    private readonly Mock<IActivityLogService> _activityLogServiceMock = new();

    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();

    private readonly Mock<IExcelExportService> _excelExportServiceMock = new();

    private readonly Mock<IPdfExportService> _pdfExportServiceMock = new();

    private readonly Mock<IRealtimeService> _realtimeServiceMock = new();

    private readonly Mock<UserManager<AppUser>> _userManagerMock;

    private readonly DealService _service;

    public DealService_CreateTests()
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

        var dealBusinessRules = new DealBusinessRules(
            _dealRepositoryMock.Object,
            _userManagerMock.Object,
            customerBusinessRules,
            authBusinessRules);

        _currentUserServiceMock
            .Setup(x => x.UserId)
            .Returns(1);

        _currentUserServiceMock
            .Setup(x => x.IsInRole("Admin"))
            .Returns(true);

        _service = new DealService(
            _dealRepositoryMock.Object,
            _unitOfWorkMock.Object,
            dealBusinessRules,
            _createValidatorMock.Object,
            _updateValidatorMock.Object,
            _activityLogServiceMock.Object,
            _currentUserServiceMock.Object,
            _excelExportServiceMock.Object,
            _pdfExportServiceMock.Object,
            _realtimeServiceMock.Object);
    }
    [Fact]
    public async Task CreateAsync_Should_Create_Deal_Successfully()
    {
        // Arrange
        var dto = new CreateDealDto
        {
            Title = "CRM Project",
            Description = "CRM Description",
            Amount = 10000,
            CustomerId = 1,
            AssignedUserId = 1
        };

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(dto.CustomerId, false))
            .ReturnsAsync(new Customer { Id = dto.CustomerId });

        _userManagerMock
            .Setup(x => x.FindByIdAsync(dto.AssignedUserId.ToString()!))
            .ReturnsAsync(new AppUser { Id = dto.AssignedUserId!.Value });

        _dealRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Deal, bool>>>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _dealRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Deal>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);

        _activityLogServiceMock.Verify(
            x => x.AddAsync(
                ActivityAction.Create,
                nameof(Deal),
                It.IsAny<int>(),
                It.IsAny<string>(),
                1),
            Times.Once);

        _realtimeServiceMock.Verify(
            x => x.DashboardUpdatedAsync(),
            Times.Once);
    }
    [Fact]
    public async Task CreateAsync_Should_Map_Dto_To_Deal()
    {
        // Arrange
        var dto = new CreateDealDto
        {
            Title = "ERP",
            Description = "ERP Description",
            Amount = 25000,
            CustomerId = 1,
            AssignedUserId = 1
        };

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(new Customer { Id = 1 });

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(new AppUser { Id = 1 });

        _dealRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Deal, bool>>>()))
            .ReturnsAsync(false);

        Deal? createdDeal = null;

        _dealRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Deal>()))
            .Callback<Deal>(x => createdDeal = x);

        // Act
        await _service.CreateAsync(dto);

        // Assert
        createdDeal.Should().NotBeNull();

        createdDeal!.Title.Should().Be(dto.Title);
        createdDeal.Description.Should().Be(dto.Description);
        createdDeal.Amount.Should().Be(dto.Amount);
        createdDeal.CustomerId.Should().Be(dto.CustomerId);
        createdDeal.AssignedUserId.Should().Be(dto.AssignedUserId);

        // Servisin set ettiği alan
        createdDeal.Stage.Should().Be(DealStage.New);
    }
    [Fact]
    public async Task CreateAsync_Should_Throw_When_Duplicate_Title()
    {
        // Arrange
        var dto = new CreateDealDto
        {
            Title = "CRM",
            CustomerId = 1,
            AssignedUserId = 1
        };

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(new Customer { Id = 1 });

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(new AppUser { Id = 1 });

        _dealRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Deal, bool>>>()))
            .ReturnsAsync(true);

        // Act
        Func<Task> act = () => _service.CreateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("An active deal with the same title already exists for this customer.");
    }
    [Fact]
    public async Task CreateAsync_Should_Throw_When_Validation_Fails()
    {
        // Arrange
        var dto = new CreateDealDto();

        var failures = new List<ValidationFailure>
    {
        new("Title", "Title is required.")
    };

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult(failures));

        // Act
        Func<Task> act = () => _service.CreateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<ValidationException>();
    }
}