using FluentValidation.TestHelper;
using SalesFlow.Business.Dtos.NoteDtos;
using SalesFlow.Business.Validations.NoteValidators;
using Xunit;

namespace SalesFlow.UnitTests.Validators.NoteValidators;

public class CreateNoteValidatorTests
{
    private readonly CreateNoteValidator _validator;

    public CreateNoteValidatorTests()
    {
        _validator = new CreateNoteValidator();
    }

    private static CreateNoteDto CreateValidDto()
    {
        return new CreateNoteDto
        {
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