using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Moq;
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

public class DealService_UpdateDeleteTests
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

    public DealService_UpdateDeleteTests()
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

        _currentUserServiceMock
            .Setup(x => x.UserId)
            .Returns(1);

        _currentUserServiceMock
            .Setup(x => x.IsInRole("Admin"))
            .Returns(true);

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
    public async Task UpdateAsync_Should_Update_Deal_Successfully()
    {
        // Arrange
        var dto = new UpdateDealDto
        {
            Id = 1,
            Title = "Updated Deal",
            CustomerId = 1,
            AssignedUserId = 1,
            Stage = DealStage.Qualified
        };

        var deal = new Deal
        {
            Id = 1,
            Title = "Old Deal",
            CustomerId = 1,
            AssignedUserId = 1,
            Stage = DealStage.New
        };

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _dealRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(deal);

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(new Customer { Id = 1 });

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(new AppUser { Id = 1 });

        _dealRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Deal, bool>>>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.UpdateAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _dealRepositoryMock.Verify(
            x => x.Update(deal),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);

        _activityLogServiceMock.Verify(
            x => x.AddAsync(
                ActivityAction.Update,
                nameof(Deal),
                deal.Id,
                It.IsAny<string>(),
                1),
            Times.Once);

        _realtimeServiceMock.Verify(
            x => x.DashboardUpdatedAsync(),
            Times.Once);
    }
    [Fact]
    public async Task UpdateAsync_Should_Update_Properties()
    {
        // Arrange
        var dto = new UpdateDealDto
        {
            Id = 1,
            Title = "CRM Updated",
            Description = "New Description",
            Amount = 75000,
            CustomerId = 1,
            AssignedUserId = 1,
            Stage = DealStage.Qualified
        };

        var deal = new Deal
        {
            Id = 1,
            Title = "Old",
            Stage = DealStage.New
        };

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _dealRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(deal);

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(new Customer { Id = 1 });

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(new AppUser { Id = 1 });

        _dealRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Deal, bool>>>()))
            .ReturnsAsync(false);

        // Act
        await _service.UpdateAsync(dto);

        // Assert
        deal.Title.Should().Be(dto.Title);
        deal.Description.Should().Be(dto.Description);
        deal.Amount.Should().Be(dto.Amount);
        deal.Stage.Should().Be(dto.Stage);
        deal.CustomerId.Should().Be(dto.CustomerId);
        deal.AssignedUserId.Should().Be(dto.AssignedUserId);
    }
    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Validation_Fails()
    {
        // Arrange
        var dto = new UpdateDealDto();

        var failures = new List<ValidationFailure>
    {
        new("Title", "Title is required.")
    };

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult(failures));

        // Act
        Func<Task> act = () => _service.UpdateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<ValidationException>();
    }
    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Deal_Not_Found()
    {
        // Arrange
        var dto = new UpdateDealDto
        {
            Id = 1
        };

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _dealRepositoryMock
            .Setup(x => x.GetByIdAsync(dto.Id, true))
            .ReturnsAsync((Deal?)null);

        // Act
        Func<Task> act = () => _service.UpdateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("Deal not found.");
    }
    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Duplicate_Title()
    {
        // Arrange
        var dto = new UpdateDealDto
        {
            Id = 1,
            Title = "CRM",
            CustomerId = 1,
            AssignedUserId = 1,
            Stage = DealStage.New
        };

        var deal = new Deal
        {
            Id = 1,
            CustomerId = 1,
            AssignedUserId = 1,
            Stage = DealStage.New
        };

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _dealRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(deal);

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
        Func<Task> act = () => _service.UpdateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("An active deal with the same title already exists for this customer.");
    }
    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Deal_Is_Not_Editable()
    {
        // Arrange
        var dto = new UpdateDealDto
        {
            Id = 1,
            CustomerId = 1,
            AssignedUserId = 1,
            Stage = DealStage.Won
        };

        var deal = new Deal
        {
            Id = 1,
            CustomerId = 1,
            AssignedUserId = 1,
            Stage = DealStage.Won
        };

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _dealRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(deal);

        // Act
        Func<Task> act = () => _service.UpdateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("Completed deals cannot be updated.");
    }
    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Invalid_Stage_Transition()
    {
        // Arrange
        var dto = new UpdateDealDto
        {
            Id = 1,
            Title = "CRM",
            CustomerId = 1,
            AssignedUserId = 1,
            Stage = DealStage.Won
        };

        var deal = new Deal
        {
            Id = 1,
            Title = "CRM",
            CustomerId = 1,
            AssignedUserId = 1,
            Stage = DealStage.New
        };

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _dealRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(deal);

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(new Customer { Id = 1 });

        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(new AppUser { Id = 1 });

        _dealRepositoryMock
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Deal, bool>>>()))
            .ReturnsAsync(false);

        // Act
        Func<Task> act = () => _service.UpdateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("Invalid deal stage transition.");
    }
    [Fact]
    public async Task DeleteAsync_Should_Delete_Deal_Successfully()
    {
        // Arrange
        var deal = new Deal
        {
            Id = 1,
            Title = "CRM",
            AssignedUserId = 1,
            Stage = DealStage.New
        };

        _dealRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(deal);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _dealRepositoryMock.Verify(
            x => x.Delete(deal),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);

        _activityLogServiceMock.Verify(
            x => x.AddAsync(
                ActivityAction.Delete,
                nameof(Deal),
                deal.Id,
                It.IsAny<string>(),
                1),
            Times.Once);

        _realtimeServiceMock.Verify(
            x => x.DashboardUpdatedAsync(),
            Times.Once);
    }
    [Fact]
    public async Task DeleteAsync_Should_Throw_When_Deal_Not_Found()
    {
        // Arrange
        _dealRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync((Deal?)null);

        // Act
        Func<Task> act = () => _service.DeleteAsync(1);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("Deal not found.");
    }
    [Fact]
    public async Task DeleteAsync_Should_Throw_When_Deal_Is_Not_Deletable()
    {
        // Arrange
        var deal = new Deal
        {
            Id = 1,
            AssignedUserId = 1,
            Stage = DealStage.Won
        };

        _dealRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(deal);

        // Act
        Func<Task> act = () => _service.DeleteAsync(1);

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("Won deals cannot be deleted.");
    }
    [Fact]
    public async Task DeleteAsync_Should_Call_DashboardUpdated()
    {
        // Arrange
        var deal = new Deal
        {
            Id = 1,
            AssignedUserId = 1,
            Stage = DealStage.New
        };

        _dealRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(deal);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _realtimeServiceMock.Verify(
            x => x.DashboardUpdatedAsync(),
            Times.Once);
    }
    [Fact]
    public async Task DeleteAsync_Should_Create_ActivityLog()
    {
        // Arrange
        var deal = new Deal
        {
            Id = 1,
            Title = "CRM",
            AssignedUserId = 1,
            Stage = DealStage.New
        };

        _dealRepositoryMock
            .Setup(x => x.GetByIdAsync(1, true))
            .ReturnsAsync(deal);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _activityLogServiceMock.Verify(
            x => x.AddAsync(
                ActivityAction.Delete,
                nameof(Deal),
                deal.Id,
                It.IsAny<string>(),
                1),
            Times.Once);
    }
}