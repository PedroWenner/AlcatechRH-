using FluentValidation;
using DPManagement.Application.DTOs;

namespace DPManagement.Application.Validators;

public class CentroCustoValidator : AbstractValidator<CentroCustoRequestDto>
{
    public CentroCustoValidator()
    {
        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(200).WithMessage("A descrição não pode exceder 200 caracteres.");

        RuleFor(x => x.OrgaoId)
            .NotEmpty().WithMessage("O vínculo com o órgão é obrigatório.");
    }
}
