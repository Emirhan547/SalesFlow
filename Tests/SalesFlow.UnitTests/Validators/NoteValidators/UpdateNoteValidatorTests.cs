using FluentValidation.TestHelper;
using SalesFlow.Business.Dtos.NoteDtos;
using SalesFlow.Business.Validations.NoteValidators;
using Xunit;

namespace SalesFlow.UnitTests.Validators.NoteValidators;

public class UpdateNoteValidatorTests
{
    private readonly UpdateNoteValidator _validator;

    public UpdateNoteValidatorTests()
    {
        _validator = new UpdateNoteValidator();
    }

    private static UpdateNoteDto CreateValidDto()
    {
        return new UpdateNoteDto
        {
            Id = 1,
            Content = "Customer note",
            CustomerId = 1
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

    #region Content

    [Fact]
    public void Should_HaveValidationError_When_ContentIsEmpty()
    {
        var dto = CreateValidDto();
        dto.Content = "";

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Content);
    }

    [Fact]
    public void Should_HaveValidationError_When_ContentExceedsMaxLength()
    {
        var dto = CreateValidDto();
        dto.Content = new string('A', 2001);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Content);
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
}