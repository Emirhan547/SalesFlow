using FluentValidation;
using SalesFlow.Business.Dtos.TaskItemDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Validations.TaskItemValidators
{
    public class UpdateTaskItemValidator : AbstractValidator<UpdateTaskItemDto>
    {
        public UpdateTaskItemValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);

            RuleFor(x => x.Title).NotEmpty().MaximumLength(150);

            RuleFor(x => x.DueDate).GreaterThanOrEqualTo(DateTime.Today);

            RuleFor(x => x.Priority) .IsInEnum();

            RuleFor(x => x.Status).IsInEnum();

            RuleFor(x => x.CustomerId).GreaterThan(0);

            RuleFor(x => x.Description).MaximumLength(1000).When(x => !string.IsNullOrWhiteSpace(x.Description));
        }
    }
}
