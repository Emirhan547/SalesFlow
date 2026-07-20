using FluentValidation.TestHelper;
using SalesFlow.Business.Dtos.TaskItemDtos;
using SalesFlow.Business.Validations.TaskItemValidators;
using SalesFlow.Entity.Enums;
using Xunit;

namespace SalesFlow.UnitTests.Validators.TaskItemValidators;

public class CreateTaskItemValidatorTests
{
    private readonly CreateTaskItemValidator _validator;

    public CreateTaskItemValidatorTests()
    {
        _validator = new CreateTaskItemValidator();
    }

    private static CreateTaskItemDto CreateValidDto()
    {
        return new CreateTaskItemDto
        {
            Title = "Call customer",
            DueDate = DateTime.Today.AddDays(1),
            Priority = TaskPriority.Medium,
            CustomerId = 1,
            Description = "Call the customer about the proposal."
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

    #region DueDate

    [Fact]
    public void Should_HaveValidationError_When_DueDateIsInThePast()
    {
        var dto = CreateValidDto();
        dto.DueDate = DateTime.Today.AddDays(-1);

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.DueDate);
    }

    #endregion

    #region Priority

    [Fact]
    public void Should_HaveValidationError_When_PriorityIsInvalid()
    {
        var dto = CreateValidDto();
        dto.Priority = (TaskPriority)999;

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Priority);
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