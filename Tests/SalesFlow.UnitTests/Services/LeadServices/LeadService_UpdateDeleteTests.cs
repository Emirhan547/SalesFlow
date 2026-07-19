using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using SalesFlow.Business.Dtos.LeadDtos;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Business.Services.ExportServices;
using SalesFlow.Business.Services.LeadServices;
using SalesFlow.Business.Services.RealtimeServices;
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
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SalesFlow.UnitTests.Services.LeadServices
{
    public class LeadService_UpdateDeleteTests
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
        public LeadService_UpdateDeleteTests()
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
      _pdfExportServiceMock.Object);
        }
        [Fact]
        public async Task UpdateAsync_Should_Update_Lead_Successfully()
        {
            // Arrange
            var dto = new UpdateLeadDto
            {
                Id = 1,
                FirstName = "Yeni",
                LastName = "İsim",
                Email = "yeni@test.com",
                CompanyName = "SalesFlow"
            };

            var lead = new Lead
            {
                Id = 1,
                FirstName = "Eski",
                LastName = "İsim",
                Email = "eski@test.com",
                AssignedUserId = 1
            };

            _updateValidatorMock
                .Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());

            _leadRepositoryMock
                .Setup(x => x.GetByIdAsync(1, true))
                .ReturnsAsync(lead);

            // Act
            var result = await _service.UpdateAsync(dto);

            // Assert
            result.IsSuccess.Should().BeTrue();

            lead.FirstName.Should().Be(dto.FirstName);
            lead.LastName.Should().Be(dto.LastName);
            lead.Email.Should().Be(dto.Email);

            _leadRepositoryMock.Verify(x => x.Update(lead), Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }
        [Fact]
        public async Task UpdateAsync_Should_Throw_When_Lead_NotFound()
        {
            // Arrange
            var dto = new UpdateLeadDto
            {
                Id = 1,
                Email = "test@test.com"
            };

            _updateValidatorMock
                .Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());

            _leadRepositoryMock
                .Setup(x => x.GetByIdAsync(1, true))
                .ReturnsAsync((Lead?)null);

            // Act
            Func<Task> act = () => _service.UpdateAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Lead not found.");
        }
        [Fact]
        public async Task UpdateAsync_Should_Throw_When_Email_Already_Exists()
        {
            // Arrange
            var dto = new UpdateLeadDto
            {
                Id = 1,
                Email = "test@test.com"
            };

            var lead = new Lead
            {
                Id = 1,
                AssignedUserId = 1
            };

            _updateValidatorMock
                .Setup(x => x.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());

            _leadRepositoryMock
                .Setup(x => x.GetByIdAsync(1, true))
                .ReturnsAsync(lead);

            _leadRepositoryMock
                .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Lead, bool>>>()))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = () => _service.UpdateAsync(dto);

            // Assert
            await act.Should()
                .ThrowAsync<BusinessException>()
                .WithMessage("A lead with this email already exists.");
        }
        [Fact]
        public async Task DeleteAsync_Should_Delete_Lead_Successfully()
        {
            // Arrange
            var lead = new Lead
            {
                Id = 1,
                FirstName = "Emirhan",
                LastName = "Hacıoğlu",
                AssignedUserId = 1
            };

            _leadRepositoryMock
                .Setup(x => x.GetByIdAsync(1, true))
                .ReturnsAsync(lead);

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            result.IsSuccess.Should().BeTrue();

            _leadRepositoryMock.Verify(
                x => x.Delete(lead),
                Times.Once);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);

            _activityLogServiceMock.Verify(
                x => x.AddAsync(
                    ActivityAction.Delete,
                    nameof(Lead),
                    lead.Id,
                    It.IsAny<string>(),
                    1),
                Times.Once);
        }
        [Fact]
        public async Task DeleteAsync_Should_Throw_When_Lead_NotFound()
        {
            _leadRepositoryMock
                .Setup(x => x.GetByIdAsync(1, true))
                .ReturnsAsync((Lead?)null);

            Func<Task> act = () => _service.DeleteAsync(1);

            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Lead not found.");
        }
    }
}
