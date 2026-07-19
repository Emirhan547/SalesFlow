using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Moq;
using SalesFlow.Business.Dtos.NoteDtos;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Business.Services.NoteServices;
using SalesFlow.Business.Services.RealtimeServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Repositories.NoteRepositories;
using SalesFlow.DataAccess.Repositories.TagRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;

namespace SalesFlow.UnitTests.Services.NoteServices;

public class NoteService_CreateTests
{
    private readonly Mock<INoteRepository> _noteRepositoryMock = new();
    private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
    private readonly Mock<ITagRepository> _tagRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly Mock<IValidator<CreateNoteDto>> _createValidatorMock = new();
    private readonly Mock<IValidator<UpdateNoteDto>> _updateValidatorMock = new();

    private readonly Mock<IActivityLogService> _activityLogServiceMock = new();
    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
    private readonly Mock<IRealtimeService> _realtimeServiceMock = new();

    private readonly Mock<UserManager<AppUser>> _userManagerMock;

    private readonly NoteService _service;

    public NoteService_CreateTests()
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

        var authBusinessRules =
            new AuthBusinessRules(
                _currentUserServiceMock.Object);

        var customerBusinessRules =
            new CustomerBusinessRules(
                _customerRepositoryMock.Object,
                _tagRepositoryMock.Object,
                _currentUserServiceMock.Object,
                authBusinessRules);

        var noteBusinessRules =
            new NoteBusinessRules(
                _noteRepositoryMock.Object,
                _userManagerMock.Object,
                customerBusinessRules,
                authBusinessRules);

        _service = new NoteService(
            _noteRepositoryMock.Object,
            _unitOfWorkMock.Object,
            noteBusinessRules,
            _createValidatorMock.Object,
            _updateValidatorMock.Object,
            _activityLogServiceMock.Object,
            _currentUserServiceMock.Object,
            _realtimeServiceMock.Object);
    }
    [Fact]
    public async Task CreateAsync_Should_Create_Note_Successfully()
    {
        // Arrange
        var dto = new CreateNoteDto
        {
            Content = "Test Note",
            CustomerId = 1
        };

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(new Customer());

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _noteRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Note>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }
    [Fact]
    public async Task CreateAsync_Should_Map_Dto_To_Note()
    {
        // Arrange
        var dto = new CreateNoteDto
        {
            Content = "Customer Meeting",
            CustomerId = 3
        };

        Note? createdNote = null;

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(3, false))
            .ReturnsAsync(new Customer());

        _noteRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Note>()))
            .Callback<Note>(x => createdNote = x)
            .Returns(Task.CompletedTask);

        // Act
        await _service.CreateAsync(dto);

        // Assert
        createdNote.Should().NotBeNull();
        createdNote!.Content.Should().Be(dto.Content);
        createdNote.CustomerId.Should().Be(dto.CustomerId);
    }
    [Fact]
    public async Task CreateAsync_Should_Throw_When_Validation_Fails()
    {
        // Arrange
        var dto = new CreateNoteDto();

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult(new[]
            {
            new ValidationFailure("Content", "Required")
            }));

        // Act
        Func<Task> act = () => _service.CreateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<ValidationException>();
    }
    [Fact]
    public async Task CreateAsync_Should_Throw_When_Customer_Not_Found()
    {
        // Arrange
        var dto = new CreateNoteDto
        {
            CustomerId = 5
        };

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(5, false))
            .ReturnsAsync((Customer?)null);

        // Act
        Func<Task> act = () => _service.CreateAsync(dto);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("Customer not found.");
    }
    [Fact]
    public async Task CreateAsync_Should_Create_ActivityLog()
    {
        // Arrange
        var dto = new CreateNoteDto
        {
            CustomerId = 1,
            Content = "Test"
        };

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(new Customer());

        // Act
        await _service.CreateAsync(dto);

        // Assert
        _activityLogServiceMock.Verify(
            x => x.AddAsync(
                ActivityAction.Create,
                nameof(Note),
                It.IsAny<int>(),
                "Note created.",
                1),
            Times.Once);
    }
    [Fact]
    public async Task CreateAsync_Should_Call_DashboardUpdated()
    {
        // Arrange
        var dto = new CreateNoteDto
        {
            CustomerId = 1,
            Content = "Realtime Test"
        };

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _customerRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(new Customer());

        // Act
        await _service.CreateAsync(dto);

        // Assert
        _realtimeServiceMock.Verify(
            x => x.DashboardUpdatedAsync(),
            Times.Once);
    }
}