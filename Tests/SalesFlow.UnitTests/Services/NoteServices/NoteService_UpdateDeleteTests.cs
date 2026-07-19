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
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.UnitTests.Services.NoteServices
{
    public class NoteService_UpdateDeleteTests
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

        public NoteService_UpdateDeleteTests()
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
        public async Task UpdateAsync_Should_Update_Note_Successfully()
        {
            // Arrange
            var dto = new UpdateNoteDto
            {
                Id = 1,
                Content = "Updated Note",
                CustomerId = 2
            };

            var note = new Note
            {
                Id = 1,
                Content = "Old Note",
                CustomerId = 1,
                CreatedById = 1
            };

            _updateValidatorMock
                .Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());

            _noteRepositoryMock
                .Setup(x => x.GetByIdAsync(1, true))
                .ReturnsAsync(note);

            _customerRepositoryMock
                .Setup(x => x.GetByIdAsync(2, false))
                .ReturnsAsync(new Customer());

            // Act
            var result = await _service.UpdateAsync(dto);

            // Assert
            result.IsSuccess.Should().BeTrue();

            _noteRepositoryMock.Verify(
                x => x.Update(note),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }
        [Fact]
        public async Task UpdateAsync_Should_Map_Dto_To_Note()
        {
            // Arrange
            var dto = new UpdateNoteDto
            {
                Id = 1,
                Content = "Updated Content",
                CustomerId = 5
            };

            var note = new Note
            {
                Id = 1,
                Content = "Old",
                CustomerId = 1,
                CreatedById = 1
            };

            _updateValidatorMock
                .Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());

            _noteRepositoryMock
                .Setup(x => x.GetByIdAsync(1, true))
                .ReturnsAsync(note);

            _customerRepositoryMock
                .Setup(x => x.GetByIdAsync(5, false))
                .ReturnsAsync(new Customer());

            // Act
            await _service.UpdateAsync(dto);

            // Assert
            note.Content.Should().Be(dto.Content);
            note.CustomerId.Should().Be(dto.CustomerId);
        }
        [Fact]
        public async Task UpdateAsync_Should_Throw_When_Validation_Fails()
        {
            // Arrange
            var dto = new UpdateNoteDto();

            _updateValidatorMock
                .Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult(new[]
                {
            new ValidationFailure("Content", "Required")
                }));

            // Act
            Func<Task> act = () => _service.UpdateAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<ValidationException>();
        }
        [Fact]
        public async Task UpdateAsync_Should_Throw_When_Note_Not_Found()
        {
            // Arrange
            var dto = new UpdateNoteDto
            {
                Id = 1
            };

            _updateValidatorMock
                .Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());

            _noteRepositoryMock
                .Setup(x => x.GetByIdAsync(1, true))
                .ReturnsAsync((Note?)null);

            // Act
            Func<Task> act = () => _service.UpdateAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Note not found.");
        }
        [Fact]
        public async Task UpdateAsync_Should_Throw_When_Customer_Not_Found()
        {
            // Arrange
            var dto = new UpdateNoteDto
            {
                Id = 1,
                CustomerId = 99
            };

            var note = new Note
            {
                Id = 1,
                CreatedById = 1
            };

            _updateValidatorMock
                .Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());

            _noteRepositoryMock
                .Setup(x => x.GetByIdAsync(1, true))
                .ReturnsAsync(note);

            _customerRepositoryMock
                .Setup(x => x.GetByIdAsync(99, false))
                .ReturnsAsync((Customer?)null);

            // Act
            Func<Task> act = () => _service.UpdateAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Customer not found.");
        }
        [Fact]
        public async Task UpdateAsync_Should_Throw_When_User_Has_No_Access()
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

            var dto = new UpdateNoteDto
            {
                Id = 1,
                CustomerId = 1
            };

            var note = new Note
            {
                Id = 1,
                CreatedById = 5
            };

            _updateValidatorMock
                .Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());

            _noteRepositoryMock
                .Setup(x => x.GetByIdAsync(1, true))
                .ReturnsAsync(note);

            // Act
            Func<Task> act = () => _service.UpdateAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<ForbiddenException>()
                .WithMessage("You are not authorized to access this record.");
        }
        [Fact]
        public async Task UpdateAsync_Should_Create_ActivityLog()
        {
            // Arrange
            var dto = new UpdateNoteDto
            {
                Id = 1,
                CustomerId = 1
            };

            var note = new Note
            {
                Id = 1,
                CreatedById = 1
            };

            _updateValidatorMock
                .Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());

            _noteRepositoryMock
                .Setup(x => x.GetByIdAsync(1, true))
                .ReturnsAsync(note);

            _customerRepositoryMock
                .Setup(x => x.GetByIdAsync(1, false))
                .ReturnsAsync(new Customer());

            // Act
            await _service.UpdateAsync(dto);

            // Assert
            _activityLogServiceMock.Verify(
                x => x.AddAsync(
                    ActivityAction.Update,
                    nameof(Note),
                    note.Id,
                    "Note updated.",
                    1),
                Times.Once);
        }
        [Fact]
        public async Task DeleteAsync_Should_Delete_Note_Successfully()
        {
            // Arrange
            var note = new Note
            {
                Id = 1,
                CreatedById = 1
            };

            _noteRepositoryMock
                .Setup(x => x.GetByIdAsync(1, true))
                .ReturnsAsync(note);

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            result.IsSuccess.Should().BeTrue();

            _noteRepositoryMock.Verify(
                x => x.Delete(note),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }
        [Fact]
        public async Task DeleteAsync_Should_Throw_When_Note_Not_Found()
        {
            // Arrange
            _noteRepositoryMock
                .Setup(x => x.GetByIdAsync(1, true))
                .ReturnsAsync((Note?)null);

            // Act
            Func<Task> act = () => _service.DeleteAsync(1);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Note not found.");
        }
        [Fact]
        public async Task DeleteAsync_Should_Throw_When_User_Has_No_Access()
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
                Id = 1,
                CreatedById = 10
            };

            _noteRepositoryMock
                .Setup(x => x.GetByIdAsync(1, true))
                .ReturnsAsync(note);

            // Act
            Func<Task> act = () => _service.DeleteAsync(1);

            // Assert
            await act.Should()
                .ThrowAsync<ForbiddenException>()
                .WithMessage("You are not authorized to access this record.");
        }
        [Fact]
        public async Task DeleteAsync_Should_Create_ActivityLog()
        {
            // Arrange
            var note = new Note
            {
                Id = 1,
                CreatedById = 1
            };

            _noteRepositoryMock
                .Setup(x => x.GetByIdAsync(1, true))
                .ReturnsAsync(note);

            // Act
            await _service.DeleteAsync(1);

            // Assert
            _activityLogServiceMock.Verify(
                x => x.AddAsync(
                    ActivityAction.Delete,
                    nameof(Note),
                    note.Id,
                    "Note deleted.",
                    1),
                Times.Once);
        }
        [Fact]
        public async Task DeleteAsync_Should_Call_DashboardUpdated()
        {
            // Arrange
            var note = new Note
            {
                Id = 1,
                CreatedById = 1
            };

            _noteRepositoryMock
                .Setup(x => x.GetByIdAsync(1, true))
                .ReturnsAsync(note);

            // Act
            await _service.DeleteAsync(1);

            // Assert
            _realtimeServiceMock.Verify(
                x => x.DashboardUpdatedAsync(),
                Times.Once);
        }
    }
}

