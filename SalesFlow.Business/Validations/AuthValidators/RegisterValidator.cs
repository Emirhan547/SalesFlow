using FluentValidation;
using SalesFlow.Business.Dtos.AuthDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Validations.AuthValidators
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.FirstName) .NotEmpty() .MaximumLength(50);

            RuleFor(x => x.LastName) .NotEmpty() .MaximumLength(50);

            RuleFor(x => x.UserName).NotEmpty().MinimumLength(3).MaximumLength(30);

            RuleFor(x => x.Email) .NotEmpty().EmailAddress();

            RuleFor(x => x.Password).NotEmpty() .MinimumLength(6);

            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords do not match.");
        }
    }
}
