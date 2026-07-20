using FluentValidation.TestHelper;
using SalesFlow.Business.Dtos.CustomerDtos;
using SalesFlow.Business.Validations.CustomerValidators;
using SalesFlow.Entity.Enums;
using Xunit;

namespace SalesFlow.UnitTests.Validators.CustomerValidators;

public class UpdateCustomerValidatorTests
{
    private readonly UpdateCustomerValidator _validator;

    public UpdateCustomerValidatorTests()
    {
        _validator = new UpdateCustomerValidator();
    }

    private static UpdateCustomerDto CreateValidDto()
    {
        return new UpdateCustomerDto
        {
            Id = 1,
            CustomerType = CustomerType.Individual,
            ContactFirstName = "Emirhan",
            ContactLastName = "Hacıoğlu",
            Email = "test@test.com",
            PhoneNumber = "5551234567",
            CompanyName = null,
            Website = null,
            TaxNumber = null,
            Address = null,
            Description = null
        };
    }

    #region Valid Model

    [Fact]
    public void Should_NotHaveValidationErrors_When_ModelIsValid()
    {
        var dto = CreateValidDto();

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    #endregion

    #region Id

    [Fact]
    public void Should_HaveValidationError_When_IdIsZero()
    {
        var dto = CreateValidDto();
        dto.Id = 0;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_HaveValidationError_When_IdIsNegative()
    {
        var dto = CreateValidDto();
        dto.Id = -1;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_NotHaveValidationError_When_IdIsGreaterThanZero()
    {
        var dto = CreateValidDto();
        dto.Id = 1;

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    #endregion

    #region CustomerType

    [Fact]
    public void Should_HaveValidationError_When_CustomerTypeIsInvalid()
    {
        var dto = CreateValidDto();
        dto.CustomerType = (CustomerType)999;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.CustomerType);
    }

    #endregion

    #region ContactFirstName

    [Fact]
    public void Should_HaveValidationError_When_ContactFirstNameIsEmpty()
    {
        var dto = CreateValidDto();
        dto.ContactFirstName = "";

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.ContactFirstName);
    }

    [Fact]
    public void Should_HaveValidationError_When_ContactFirstNameExceedsMaxLength()
    {
        var dto = CreateValidDto();
        dto.ContactFirstName = new string('A', 51);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.ContactFirstName);
    }

    #endregion

    #region ContactLastName

    [Fact]
    public void Should_HaveValidationError_When_ContactLastNameIsEmpty()
    {
        var dto = CreateValidDto();
        dto.ContactLastName = "";

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.ContactLastName);
    }

    [Fact]
    public void Should_HaveValidationError_When_ContactLastNameExceedsMaxLength()
    {
        var dto = CreateValidDto();
        dto.ContactLastName = new string('A', 51);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.ContactLastName);
    }

    #endregion

    #region Email

    [Fact]
    public void Should_HaveValidationError_When_EmailIsEmpty()
    {
        var dto = CreateValidDto();
        dto.Email = "";

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_HaveValidationError_When_EmailIsInvalid()
    {
        var dto = CreateValidDto();
        dto.Email = "invalid-email";

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_HaveValidationError_When_EmailExceedsMaxLength()
    {
        var dto = CreateValidDto();
        dto.Email = new string('a', 95) + "@test.com";

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    #endregion

    #region PhoneNumber

    [Fact]
    public void Should_HaveValidationError_When_PhoneNumberIsEmpty()
    {
        var dto = CreateValidDto();
        dto.PhoneNumber = "";

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Fact]
    public void Should_HaveValidationError_When_PhoneNumberExceedsMaxLength()
    {
        var dto = CreateValidDto();
        dto.PhoneNumber = new string('1', 21);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    #endregion

    #region CompanyName

    [Fact]
    public void Should_HaveValidationError_When_CompanyNameIsEmpty_ForCompanyCustomer()
    {
        var dto = CreateValidDto();

        dto.CustomerType = CustomerType.Company;
        dto.CompanyName = "";

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.CompanyName);
    }

    [Fact]
    public void Should_NotHaveValidationError_When_CompanyNameExists_ForCompanyCustomer()
    {
        var dto = CreateValidDto();

        dto.CustomerType = CustomerType.Company;
        dto.CompanyName = "OpenAI";

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.CompanyName);
    }

    [Fact]
    public void Should_HaveValidationError_When_CompanyNameExceedsMaxLength()
    {
        var dto = CreateValidDto();
        dto.CompanyName = new string('A', 151);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.CompanyName);
    }

    #endregion

    #region Website

    [Fact]
    public void Should_HaveValidationError_When_WebsiteIsInvalid()
    {
        var dto = CreateValidDto();
        dto.Website = "website";

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Website);
    }

    [Fact]
    public void Should_HaveValidationError_When_WebsiteExceedsMaxLength()
    {
        var dto = CreateValidDto();
        dto.Website = "https://" + new string('a', 250) + ".com";

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Website);
    }

    #endregion

    #region TaxNumber

    [Fact]
    public void Should_HaveValidationError_When_TaxNumberExceedsMaxLength()
    {
        var dto = CreateValidDto();
        dto.TaxNumber = new string('1', 21);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.TaxNumber);
    }

    #endregion

    #region Address

    [Fact]
    public void Should_HaveValidationError_When_AddressExceedsMaxLength()
    {
        var dto = CreateValidDto();
        dto.Address = new string('A', 301);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Address);
    }

    #endregion

    #region Description

    [Fact]
    public void Should_HaveValidationError_When_DescriptionExceedsMaxLength()
    {
        var dto = CreateValidDto();
        dto.Description = new string('A', 1001);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    #endregion
}