using FluentValidation;
using DPManagement.Application.DTOs;

namespace DPManagement.Application.Validators;

public class OrgaoValidator : AbstractValidator<OrgaoRequestDto>
{
    public OrgaoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(200).WithMessage("O nome não pode exceder 200 caracteres.");

        RuleFor(x => x.Abreviatura)
            .NotEmpty().WithMessage("A abreviatura é obrigatória.")
            .MaximumLength(30).WithMessage("A abreviatura não pode exceder 30 caracteres.");

        RuleFor(x => x.Nivel)
            .InclusiveBetween(1, 3).WithMessage("O nível hierárquico deve ser entre 1 e 3.");

        RuleFor(x => x.OrgaoPaiId)
            .NotEmpty().When(x => x.Nivel > 1)
            .WithMessage("Para secretarias e departamentos, o órgão pai é obrigatório.");
    }
}
