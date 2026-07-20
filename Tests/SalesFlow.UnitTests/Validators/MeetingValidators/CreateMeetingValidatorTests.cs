using FluentValidation.TestHelper;
using SalesFlow.Business.Dtos.MeetingDtos;
using SalesFlow.Business.Validations.MeetingValidators;
using SalesFlow.Entity.Enums;
using Xunit;

namespace SalesFlow.UnitTests.Validators.MeetingValidators;

public class CreateMeetingValidatorTests
{
    private readonly CreateMeetingValidator _validator;

    public CreateMeetingValidatorTests()
    {
        _validator = new CreateMeetingValidator();
    }

    private static CreateMeetingDto CreateValidDto()
    {
        return new CreateMeetingDto
        {
            Title = "Customer Meeting",
            StartDate = DateTime.Today.AddDays(1).AddHours(9),
            EndDate = DateTime.Today.AddDays(1).AddHours(10),
            Type = MeetingType.Online,
            CustomerId = 1,
            Location = "Microsoft Teams",
            Description = "Project discussion"
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

    #region Title

    [Fact]
    public void Should_HaveValidationError_When_TitleIsEmpty()
    {
        var dto = CreateValidDto();
        dto.Title = "";

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_HaveValidationError_When_TitleExceedsMaxLength()
    {
        var dto = CreateValidDto();
        dto.Title = new string('A', 151);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    #endregion

    #region StartDate

    [Fact]
    public void Should_HaveValidationError_When_StartDateIsDefault()
    {
        var dto = CreateValidDto();
        dto.StartDate = default;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.StartDate);
    }

    #endregion

    #region EndDate

    [Fact]
    public void Should_HaveValidationError_When_EndDateIsDefault()
    {
        var dto = CreateValidDto();
        dto.EndDate = default;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    [Fact]
    public void Should_HaveValidationError_When_EndDateIsEarlierThanStartDate()
    {
        var dto = CreateValidDto();
        dto.EndDate = dto.StartDate.AddMinutes(-30);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    [Fact]
    public void Should_HaveValidationError_When_EndDateEqualsStartDate()
    {
        var dto = CreateValidDto();
        dto.EndDate = dto.StartDate;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    #endregion

    #region Type

    [Fact]
    public void Should_HaveValidationError_When_TypeIsInvalid()
    {
        var dto = CreateValidDto();
        dto.Type = (MeetingType)999;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Type);
    }

    #endregion

    #region CustomerId

    [Fact]
    public void Should_HaveValidationError_When_CustomerIdIsZero()
    {
        var dto = CreateValidDto();
        dto.CustomerId = 0;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.CustomerId);
    }

    [Fact]
    public void Should_HaveValidationError_When_CustomerIdIsNegative()
    {
        var dto = CreateValidDto();
        dto.CustomerId = -1;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.CustomerId);
    }

    #endregion

    #region Location

    [Fact]
    public void Should_HaveValidationError_When_LocationExceedsMaxLength()
    {
        var dto = CreateValidDto();
        dto.Location = new string('A', 201);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Location);
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