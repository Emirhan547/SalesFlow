using FluentValidation;
using SalesFlow.Business.Dtos.NoteDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Validations.NoteValidators
{
    public class CreateNoteValidator : AbstractValidator<CreateNoteDto>
    {
        public CreateNoteValidator()
        {
            RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required.").MaximumLength(2000);

            RuleFor(x => x.CustomerId).GreaterThan(0);
        }
    }
}
