using FluentValidation;
using DPManagement.Application.DTOs;

namespace DPManagement.Application.Validators;

public class CreateCargoValidator : AbstractValidator<CreateCargoDto>
{
    public CreateCargoValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(150);
        RuleFor(x => x.CBO).NotEmpty().MaximumLength(10);
    }
}
