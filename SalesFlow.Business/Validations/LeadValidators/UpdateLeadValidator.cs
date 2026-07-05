using FluentValidation;
using SalesFlow.Business.Dtos.LeadDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Validations.LeadValidators
{
    public class UpdateLeadValidator : AbstractValidator<UpdateLeadDto>
    {
        public UpdateLeadValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Invalid lead id.");

            RuleFor(x => x.Status) .IsInEnum();

            RuleFor(x => x.Source).IsInEnum();

            RuleFor(x => x.FirstName).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("First name is required.").MaximumLength(50);

            RuleFor(x => x.LastName).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("Last name is required.").MaximumLength(50);

            RuleFor(x => x.Email).Cascade(CascadeMode.Stop).NotEmpty().WithMessage("Email is required.") .EmailAddress().WithMessage("Please enter a valid email address.").MaximumLength(100);

            RuleFor(x => x.PhoneNumber) .Cascade(CascadeMode.Stop).NotEmpty().WithMessage("Phone number is required.") .MaximumLength(20);

            When(x => !string.IsNullOrWhiteSpace(x.CompanyName), () =>
            {
                RuleFor(x => x.CompanyName)
                    .MaximumLength(150);
            });

            When(x => !string.IsNullOrWhiteSpace(x.Website), () =>
            {
                RuleFor(x => x.Website)
                    .MaximumLength(200);
            });

            When(x => !string.IsNullOrWhiteSpace(x.Address), () =>
            {
                RuleFor(x => x.Address)
                    .MaximumLength(300);
            });

            When(x => !string.IsNullOrWhiteSpace(x.Description), () =>
            {
                RuleFor(x => x.Description)
                    .MaximumLength(1000);
            });

            When(x => x.Source == Entity.Enums.LeadSource.Website, () =>
            {
                RuleFor(x => x.Website)
                    .NotEmpty()
                    .WithMessage("Website is required when lead source is Website.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.CompanyName), () =>
            {
                RuleFor(x => x.CompanyName)
                    .MaximumLength(150);
            });
        }
    }
}
