using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Business.Services.NoteServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Repositories.NoteRepositories;
using SalesFlow.DataAccess.Repositories.TagRepositories;
using SalesFlow.Entity.Entities;

namespace SalesFlow.UnitTests.BusinessRules;

public class NoteBusinessRulesTests
{
    private readonly Mock<INoteRepository> _noteRepositoryMock = new();
    private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
    private readonly Mock<ITagRepository> _tagRepositoryMock = new();

    private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();

    private readonly Mock<UserManager<AppUser>> _userManagerMock;

    private readonly NoteBusinessRules _businessRules;

    public NoteBusinessRulesTests()
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

        _businessRules =
            new NoteBusinessRules(
                _noteRepositoryMock.Object,
                _userManagerMock.Object,
                customerBusinessRules,
                authBusinessRules);
    }
    [Fact]
    public async Task GetNoteByIdAsync_Should_Return_Note()
    {
        // Arrange
        var note = new Note
        {
            Id = 1
        };

        _noteRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync(note);

        // Act
        var result = await _businessRules.GetNoteByIdAsync(1);

        // Assert
        result.Should().Be(note);
    }
    [Fact]
    public async Task GetNoteByIdAsync_Should_Throw_When_NotFound()
    {
        // Arrange
        _noteRepositoryMock
            .Setup(x => x.GetByIdAsync(1, false))
            .ReturnsAsync((Note?)null);

        // Act
        Func<Task> act = () => _businessRules.GetNoteByIdAsync(1);

        // Assert
        await act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage("Note not found.");
    }
    [Fact]
    public async Task EnsureCreatedByExistsAsync_Should_Not_Throw()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync(new AppUser { Id = 1 });

        // Act
        await _businessRules.EnsureCreatedByExistsAsync(1);

        // Assert
    }
    [Fact]
    public async Task EnsureCreatedByExistsAsync_Should_Not_Throw_When_UserId_Is_Null()
    {
        // Act
        await _businessRules.EnsureCreatedByExistsAsync(null);

        // Assert
    }
    [Fact]
    public async Task EnsureCreatedByExistsAsync_Should_Throw()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.FindByIdAsync("1"))
            .ReturnsAsync((AppUser?)null);

        // Act
        Func<Task> act =
            () => _businessRules.EnsureCreatedByExistsAsync(1);

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("User not found.");
    }
    [Fact]
    public void EnsureUserCanModify_Should_Not_Throw()
    {
        // Arrange
        var note = new Note
        {
            CreatedById = 1
        };

        // Act
        var act = () => _businessRules.EnsureUserCanModify(note);

        // Assert
        act.Should().NotThrow();
    }
    [Fact]
    public void EnsureUserCanModify_Should_Throw()
    {
        // Arrange
        _currentUserServiceMock
            .Setup(x => x.IsInRole("Admin"))
            .Returns(false);

        _currentUserServiceMock
            .Setup(x => x.IsInRole("SalesManager"))
            .Returns(false);

        _currentUserServiceMock
            .Setup(x => x.UserId)
            .Returns(1);

        var note = new Note
        {
            CreatedById = 5
        };

        // Act
        var act = () => _businessRules.EnsureUserCanModify(note);

        // Assert
        act.Should()
            .Throw<ForbiddenException>()
            .WithMessage("You are not authorized to access this record.");
    }
}