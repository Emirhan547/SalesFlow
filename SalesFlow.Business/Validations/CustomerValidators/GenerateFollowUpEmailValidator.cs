using FluentValidation;
using SalesFlow.Business.Dtos.CustomerDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Validations.CustomerValidators
{
    public class GenerateFollowUpEmailValidator
    : AbstractValidator<GenerateFollowUpEmailDto>
    {
        public GenerateFollowUpEmailValidator()
        {
            RuleFor(x => x.Tone)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(x => x.Purpose)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
