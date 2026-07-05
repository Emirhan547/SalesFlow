using FluentValidation;
using SalesFlow.Business.Dtos.AuthDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Validations.AuthValidators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty() .EmailAddress();

            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
