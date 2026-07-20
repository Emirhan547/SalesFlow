using FluentValidation.TestHelper;
using SalesFlow.Business.Dtos.LeadDtos;
using SalesFlow.Business.Validations.LeadValidators;
using SalesFlow.Entity.Enums;
using Xunit;

namespace SalesFlow.UnitTests.Validators.LeadValidators;

public class UpdateLeadValidatorTests
{
    private readonly UpdateLeadValidator _validator;

    public UpdateLeadValidatorTests()
    {
        _validator = new UpdateLeadValidator();
    }

    private static UpdateLeadDto CreateValidDto()
    {
        return new UpdateLeadDto
        {
            Id = 1,
            Status = LeadStatus.New,
            Source = LeadSource.Referral,
            FirstName = "Emirhan",
            LastName = "Hacıoğlu",
            Email = "test@test.com",
            PhoneNumber = "5551234567",
            CompanyName = null,
            Website = null,
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

    #endregion

    #region Status

    [Fact]
    public void Should_HaveValidationError_When_StatusIsInvalid()
    {
        var dto = CreateValidDto();
        dto.Status = (LeadStatus)999;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Status);
    }

    #endregion

    #region Source

    [Fact]
    public void Should_HaveValidationError_When_SourceIsInvalid()
    {
        var dto = CreateValidDto();
        dto.Source = (LeadSource)999;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Source);
    }

    #endregion

    #region FirstName

    [Fact]
    public void Should_HaveValidationError_When_FirstNameIsEmpty()
    {
        var dto = CreateValidDto();
        dto.FirstName = "";

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void Should_HaveValidationError_When_FirstNameExceedsMaxLength()
    {
        var dto = CreateValidDto();
        dto.FirstName = new string('A', 51);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    #endregion

    #region LastName

    [Fact]
    public void Should_HaveValidationError_When_LastNameIsEmpty()
    {
        var dto = CreateValidDto();
        dto.LastName = "";

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Fact]
    public void Should_HaveValidationError_When_LastNameExceedsMaxLength()
    {
        var dto = CreateValidDto();
        dto.LastName = new string('A', 51);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.LastName);
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
    public void Should_HaveValidationError_When_WebsiteExceedsMaxLength()
    {
        var dto = CreateValidDto();
        dto.Website = new string('A', 201);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Website);
    }

    [Fact]
    public void Should_HaveValidationError_When_WebsiteIsEmpty_ForWebsiteSource()
    {
        var dto = CreateValidDto();
        dto.Source = LeadSource.Website;
        dto.Website = "";

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Website);
    }

    [Fact]
    public void Should_NotHaveValidationError_When_WebsiteExists_ForWebsiteSource()
    {
        var dto = CreateValidDto();
        dto.Source = LeadSource.Website;
        dto.Website = "https://openai.com";

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Website);
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