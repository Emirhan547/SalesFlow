using FluentValidation.TestHelper;
using SalesFlow.Business.Dtos.LeadDtos;
using SalesFlow.Business.Validations.LeadValidators;
using SalesFlow.Entity.Enums;
using Xunit;

namespace SalesFlow.UnitTests.Validators.LeadValidators;

public class ConvertLeadValidatorTests
{
    private readonly ConvertLeadValidator _validator;

    public ConvertLeadValidatorTests()
    {
        _validator = new ConvertLeadValidator();
    }

    [Fact]
    public void Should_NotHaveValidationErrors_When_ModelIsValid()
    {
        var dto = new ConvertLeadDto
        {
            CustomerType = CustomerType.Individual
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_HaveValidationError_When_CustomerTypeIsInvalid()
    {
        var dto = new ConvertLeadDto
        {
            CustomerType = (CustomerType)999
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.CustomerType);
    }
}