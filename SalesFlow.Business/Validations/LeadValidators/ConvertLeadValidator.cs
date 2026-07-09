using FluentValidation;
using SalesFlow.Business.Dtos.LeadDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Validations.LeadValidators
{
    public class ConvertLeadValidator : AbstractValidator<ConvertLeadDto>
    {
        public ConvertLeadValidator()
        {
            RuleFor(x => x.CustomerType)
                .IsInEnum();
        }
    }
}
