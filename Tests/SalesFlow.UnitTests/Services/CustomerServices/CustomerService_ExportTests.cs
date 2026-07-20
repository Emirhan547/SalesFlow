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
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Repositories.TagRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;

namespace SalesFlow.UnitTests.Services.CustomerServices
{
    public class CustomerService_ExportTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
        private readonly Mock<ITagRepository> _tagRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

        private readonly Mock<IValidator<CreateCustomerDto>> _createValidatorMock = new();
        private readonly Mock<IValidator<UpdateCustomerDto>> _updateValidatorMock = new();

        private readonly Mock<IActivityLogService> _activityLogServiceMock = new();
        private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();

        private readonly Mock<IExcelExportService> _excelExportServiceMock = new();
        private readonly Mock<IPdfExportService> _pdfExportServiceMock = new();

        private readonly Mock<IRealtimeService> _realtimeServiceMock = new();
        private readonly Mock<IOpenAiService> _openAiServiceMock = new();
        private readonly CustomerService _service;

        public CustomerService_ExportTests()
        {
            var authBusinessRules = new AuthBusinessRules(
                _currentUserServiceMock.Object);

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
                _realtimeServiceMock.Object
                , _openAiServiceMock.Object);
        }

        [Fact]
        public async Task ExportAsync_Should_Return_Excel_File()
        {
            // Arrange
            var customers = new List<Customer>
    {
        new()
        {
            Id = 1,
            ContactFirstName = "Emirhan",
            AssignedUserId = 1
        }
    };

            var expectedBytes = new byte[] { 1, 2, 3 };

            _currentUserServiceMock
                .Setup(x => x.IsInRole("Admin"))
                .Returns(true);

            _customerRepositoryMock
                .Setup(x => x.GetAll(false))
                .Returns(customers.AsQueryable());

            _excelExportServiceMock
                .Setup(x => x.ExportCustomers(It.IsAny<List<Customer>>()))
                .Returns(expectedBytes);

            // Act
            var result = await _service.ExportAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedBytes);

            _excelExportServiceMock.Verify(
                x => x.ExportCustomers(It.IsAny<List<Customer>>()),
                Times.Once);
        }

        [Fact]
        public async Task ExportPdfAsync_Should_Return_Pdf_File()
        {
            // Arrange
            var customers = new List<Customer>
    {
        new()
        {
            Id = 1,
            ContactFirstName = "Emirhan",
            AssignedUserId = 1
        }
    };

            var expectedBytes = new byte[] { 5, 6, 7 };

            _currentUserServiceMock
                .Setup(x => x.IsInRole("Admin"))
                .Returns(true);

            _customerRepositoryMock
                .Setup(x => x.GetAll(false))
                .Returns(customers.AsQueryable());

            _pdfExportServiceMock
                .Setup(x => x.ExportCustomers(It.IsAny<List<Customer>>()))
                .Returns(expectedBytes);

            // Act
            var result = await _service.ExportPdfAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedBytes);

            _pdfExportServiceMock.Verify(
                x => x.ExportCustomers(It.IsAny<List<Customer>>()),
                Times.Once);
        }
    }
}