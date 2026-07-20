using FluentValidation.TestHelper;
using SalesFlow.Business.Dtos.DealDtos;
using SalesFlow.Business.Validations.DealValidators;
using SalesFlow.Entity.Enums;
using Xunit;

namespace SalesFlow.UnitTests.Validators.DealValidators;

public class UpdateDealValidatorTests
{
    private readonly UpdateDealValidator _validator;

    public UpdateDealValidatorTests()
    {
        _validator = new UpdateDealValidator();
    }

    private static UpdateDealDto CreateValidDto()
    {
        return new UpdateDealDto
        {
            Id = 1,
            Title = "CRM Project",
            Amount = 15000,
            CustomerId = 1,
            Stage = DealStage.New,
            ExpectedCloseDate = DateTime.Today.AddDays(10),
            Description = "New deal"
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

    #region Amount

    [Fact]
    public void Should_HaveValidationError_When_AmountIsZero()
    {
        var dto = CreateValidDto();
        dto.Amount = 0;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Amount);
    }

    [Fact]
    public void Should_HaveValidationError_When_AmountIsNegative()
    {
        var dto = CreateValidDto();
        dto.Amount = -100;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Amount);
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

    #region Stage

    [Fact]
    public void Should_HaveValidationError_When_StageIsInvalid()
    {
        var dto = CreateValidDto();
        dto.Stage = (DealStage)999;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Stage);
    }

    #endregion

    #region ExpectedCloseDate

    [Fact]
    public void Should_HaveValidationError_When_ExpectedCloseDateIsInThePast()
    {
        var dto = CreateValidDto();
        dto.ExpectedCloseDate = DateTime.Today.AddDays(-1);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.ExpectedCloseDate);
    }

    [Fact]
    public void Should_NotHaveValidationError_When_ExpectedCloseDateIsNull()
    {
        var dto = CreateValidDto();
        dto.ExpectedCloseDate = null;

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.ExpectedCloseDate);
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