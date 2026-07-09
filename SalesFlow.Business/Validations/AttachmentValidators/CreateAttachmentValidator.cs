using FluentValidation;
using SalesFlow.Business.Dtos.AttachmentDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Validations.AttachmentValidators
{
    public class CreateAttachmentValidator : AbstractValidator<CreateAttachmentDto>
    {
        public CreateAttachmentValidator()
        {

            RuleFor(x => x.CustomerId).GreaterThan(0);
        }
    }
}
