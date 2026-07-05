using FluentValidation;
using SalesFlow.Business.Dtos.TagDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Validations.TagValidators
{
    public class UpdateTagValidator : AbstractValidator<UpdateTagDto>
    {
        public UpdateTagValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);

            RuleFor(x => x.Name).NotEmpty().WithMessage("Tag name is required.").MaximumLength(50);

            RuleFor(x => x.Color).MaximumLength(20).When(x => !string.IsNullOrWhiteSpace(x.Color));
        }
    }
}
