using FluentValidation.TestHelper;
using SalesFlow.Business.Dtos.TagDtos;
using SalesFlow.Business.Validations.TagValidators;
using Xunit;

namespace SalesFlow.UnitTests.Validators.TagValidators;

public class UpdateTagValidatorTests
{
    private readonly UpdateTagValidator _validator;

    public UpdateTagValidatorTests()
    {
        _validator = new UpdateTagValidator();
    }

    private static UpdateTagDto CreateValidDto()
    {
        return new UpdateTagDto
        {
            Id = 1,
            Name = "Important",
            Color = "#FF0000"
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

    #region Name

    [Fact]
    public void Should_HaveValidationError_When_NameIsEmpty()
    {
        var dto = CreateValidDto();
        dto.Name = "";

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_HaveValidationError_When_NameExceedsMaxLength()
    {
        var dto = CreateValidDto();
        dto.Name = new string('A', 51);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    #endregion

    #region Color

    [Fact]
    public void Should_HaveValidationError_When_ColorExceedsMaxLength()
    {
        var dto = CreateValidDto();
        dto.Color = new string('A', 21);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Color);
    }

    [Fact]
    public void Should_NotHaveValidationError_When_ColorIsNull()
    {
        var dto = CreateValidDto();
        dto.Color = null;

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Color);
    }

    #endregion
}