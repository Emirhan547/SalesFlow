using FluentValidation;
using SalesFlow.Business.Dtos.MeetingDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Validations.MeetingValidators
{
    public class UpdateMeetingValidator:AbstractValidator<UpdateMeetingDto>
    {
        public UpdateMeetingValidator()
        {
            RuleFor(x => x.Title) .NotEmpty().WithMessage("Meeting title is required.").MaximumLength(150);

            RuleFor(x => x.StartDate).NotEmpty();

            RuleFor(x => x.EndDate).NotEmpty().GreaterThan(x => x.StartDate).WithMessage("End date must be later than start date.");

            RuleFor(x => x.Type).IsInEnum();

            RuleFor(x => x.CustomerId).GreaterThan(0);

            RuleFor(x => x.Location).MaximumLength(200).When(x => !string.IsNullOrWhiteSpace(x.Location));

            RuleFor(x => x.Description).MaximumLength(1000).When(x => !string.IsNullOrWhiteSpace(x.Description));
        }
    }
}
