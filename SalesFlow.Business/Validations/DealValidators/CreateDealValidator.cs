using FluentValidation;
using SalesFlow.Business.Dtos.DealDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Validations.DealValidators
{
    public class CreateDealValidator : AbstractValidator<CreateDealDto>
    {
        public CreateDealValidator()
        {
            RuleFor(x => x.Title).NotEmpty() .MaximumLength(150);

            RuleFor(x => x.Amount).GreaterThan(0);

            RuleFor(x => x.CustomerId).GreaterThan(0);

            RuleFor(x => x.ExpectedCloseDate).GreaterThanOrEqualTo(DateTime.Today) .When(x => x.ExpectedCloseDate.HasValue);

            RuleFor(x => x.Description).MaximumLength(1000).When(x => !string.IsNullOrWhiteSpace(x.Description));
        }
    }
}