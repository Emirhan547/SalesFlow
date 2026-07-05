using FluentValidation;
using SalesFlow.Business.Dtos.AuthDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Validations.AuthValidators
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenDto>
    {
        public RefreshTokenValidator()
        {
            RuleFor(x => x.RefreshToken) .NotEmpty();
        }
    }
}
