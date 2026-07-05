using FluentValidation;
using SalesFlow.Business.Dtos.AttachmentDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Validations.AttachmentValidators
{
    public class UpdateAttachmentValidator : AbstractValidator<UpdateAttachmentDto>
    {
        public UpdateAttachmentValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);

            RuleFor(x => x.FileName).NotEmpty().WithMessage("File name is required.").MaximumLength(255);

            RuleFor(x => x.FilePath).NotEmpty().WithMessage("File path is required.");

            RuleFor(x => x.ContentType).NotEmpty().WithMessage("Content type is required.").MaximumLength(100);

            RuleFor(x => x.FileSize) .GreaterThan(0).WithMessage("File size must be greater than zero.");

            RuleFor(x => x.CustomerId) .GreaterThan(0);
        }
    }
}
