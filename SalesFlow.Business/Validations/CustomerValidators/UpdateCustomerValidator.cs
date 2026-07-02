using FluentValidation;
using SalesFlow.Business.Dtos.CustomerDtos;
using SalesFlow.Entity.Enums;

namespace SalesFlow.Business.Validations.CustomerValidators
{
    public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerDto>
    {
        public UpdateCustomerValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid customer id.");

            RuleFor(x => x.CustomerType)
                .IsInEnum();

            RuleFor(x => x.ContactFirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50);

            RuleFor(x => x.ContactLastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50);

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.")
                .MaximumLength(100);

            RuleFor(x => x.PhoneNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Phone number is required.")
                .MaximumLength(20);

            When(x => !string.IsNullOrWhiteSpace(x.CompanyName), () =>
            {
                RuleFor(x => x.CompanyName)
                    .MaximumLength(150);
            });

            When(x => !string.IsNullOrWhiteSpace(x.Website), () =>
            {
                RuleFor(x => x.Website)
                    .MaximumLength(200)
                    .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                    .WithMessage("Website must be a valid URL.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.TaxNumber), () =>
            {
                RuleFor(x => x.TaxNumber)
                    .MaximumLength(20);
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

            When(x => x.CustomerType == CustomerType.Company, () =>
            {
                RuleFor(x => x.CompanyName)
                    .NotEmpty()
                    .WithMessage("Company name is required for company customers.");
            });
        }
    }
}