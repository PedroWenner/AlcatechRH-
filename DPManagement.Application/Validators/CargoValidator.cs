using FluentValidation;
using DPManagement.Application.DTOs;

namespace DPManagement.Application.Validators;

public class CreateCargoValidator : AbstractValidator<CreateCargoDto>
{
    public CreateCargoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do cargo é obrigatório.")
            .MaximumLength(150).WithMessage("O nome não pode exceder 150 caracteres.");

        RuleFor(x => x.CBO)
            .NotEmpty().WithMessage("O CBO é obrigatório.")
            .MaximumLength(10).WithMessage("O CBO não pode exceder 10 caracteres.");
    }
}

public class UpdateCargoValidator : AbstractValidator<UpdateCargoDto>
{
    public UpdateCargoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O ID do cargo é obrigatório.");

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do cargo é obrigatório.")
            .MaximumLength(150).WithMessage("O nome não pode exceder 150 caracteres.");

        RuleFor(x => x.CBO)
            .NotEmpty().WithMessage("O CBO é obrigatório.")
            .MaximumLength(10).WithMessage("O CBO não pode exceder 10 caracteres.");
    }
}
