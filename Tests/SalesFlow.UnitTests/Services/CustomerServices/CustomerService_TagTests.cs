using FluentAssertions;
using FluentValidation;
using Moq;
using SalesFlow.Business.Dtos.CustomerDtos;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Business.Services.AIServices;
using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Business.Services.ExportServices;
using SalesFlow.Business.Services.RealtimeServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Repositories.TagRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.UnitTests.Services.CustomerServices
{
    public class CustomerService_TagTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

        private readonly Mock<IValidator<CreateCustomerDto>> _createValidatorMock = new();
        private readonly Mock<IValidator<UpdateCustomerDto>> _updateValidatorMock = new();

        private readonly Mock<IActivityLogService> _activityLogServiceMock = new();
        private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();

        private readonly Mock<IExcelExportService> _excelExportServiceMock = new();
        private readonly Mock<IPdfExportService> _pdfExportServiceMock = new();

        private readonly Mock<IRealtimeService> _realtimeServiceMock = new();

        private readonly Mock<ITagRepository> _tagRepositoryMock = new();

        private readonly CustomerService _service;
        private readonly Mock<IOpenAiService> _openAiServiceMock = new();

        public CustomerService_TagTests()
        {
            var authRules = new AuthBusinessRules(_currentUserServiceMock.Object);

            var customerBusinessRules = new CustomerBusinessRules(
                _customerRepositoryMock.Object,
                _tagRepositoryMock.Object,
                _currentUserServiceMock.Object,
                authRules);

            _currentUserServiceMock.Setup(x => x.IsInRole("Admin")).Returns(true);

            _service = new CustomerService(
                _customerRepositoryMock.Object,
                _unitOfWorkMock.Object,
                customerBusinessRules,
                _createValidatorMock.Object,
                _updateValidatorMock.Object,
                _activityLogServiceMock.Object,
                _currentUserServiceMock.Object,
                _excelExportServiceMock.Object,
                _pdfExportServiceMock.Object,
                _realtimeServiceMock.Object, _openAiServiceMock.Object);
        }

        [Fact]
        public async Task AddTagAsync_Should_Add_Tag_To_Customer()
        {
            var customer = new Customer
            {
                Id = 1,
                CustomerTags = new List<CustomerTag>()
            };

            _customerRepositoryMock
                .Setup(x => x.GetByIdAsync(1, true))
                .ReturnsAsync(customer);

            _customerRepositoryMock
                .Setup(x => x.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Customer, bool>>>()))
                .ReturnsAsync(false);

            _tagRepositoryMock
                .Setup(x => x.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Tag, bool>>>()))
                .ReturnsAsync(true);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);

            var result = await _service.AddTagAsync(1, 10);

            result.IsSuccess.Should().BeTrue();

            customer.CustomerTags.Should().ContainSingle();

            customer.CustomerTags.First().TagId.Should().Be(10);

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task RemoveTagAsync_Should_Remove_Tag()
        {
            var tag = new CustomerTag
            {
                CustomerId = 1,
                TagId = 5
            };

            var customer = new Customer
            {
                Id = 1,
                CustomerTags = new List<CustomerTag>
            {
                tag
            }
            };

            _customerRepositoryMock
                .Setup(x => x.GetCustomerWithTagsAsync(1, true))
                .ReturnsAsync(customer);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);

            var result = await _service.RemoveTagAsync(1, 5);

            result.IsSuccess.Should().BeTrue();

            customer.CustomerTags.Should().BeEmpty();

            _unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);
        }

        [Fact]
        public async Task RemoveTagAsync_Should_Throw_When_Tag_Not_Assigned()
        {
            var customer = new Customer
            {
                CustomerTags = new List<CustomerTag>()
            };

            _customerRepositoryMock
                .Setup(x => x.GetCustomerWithTagsAsync(1, true))
                .ReturnsAsync(customer);

            Func<Task> act = () =>
                _service.RemoveTagAsync(1, 10);

            await act.Should()
                .ThrowAsync<BusinessException>()
                .WithMessage("Tag not assigned to this customer.");
        }

        [Fact]
        public async Task GetTagsAsync_Should_Return_Tag_List()
        {
            var customer = new Customer
            {
                CustomerTags = new List<CustomerTag>
            {
                new()
                {
                    Tag = new Tag
                    {
                        Id = 1,
                        Name = "VIP"
                    }
                },
                new()
                {
                    Tag = new Tag
                    {
                        Id = 2,
                        Name = "Important"
                    }
                }
            }
            };

            _customerRepositoryMock
                .Setup(x => x.GetCustomerWithTagsAsync(1, false))
                .ReturnsAsync(customer);

            var result = await _service.GetTagsAsync(1);

            result.IsSuccess.Should().BeTrue();

            result.Data.Should().HaveCount(2);

            result.Data.Select(x => x.Name)
                .Should()
                .Contain(new[] { "VIP", "Important" });
        }

        [Fact]
        public async Task GetTagsAsync_Should_Throw_When_Customer_NotFound()
        {
            _customerRepositoryMock
                .Setup(x => x.GetCustomerWithTagsAsync(1, false))
                .ReturnsAsync((Customer?)null);

            Func<Task> act = () =>
                _service.GetTagsAsync(1);

            await act.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Customer not found.");
        }
    }
}