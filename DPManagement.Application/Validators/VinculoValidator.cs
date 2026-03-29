using FluentValidation;
using DPManagement.Application.DTOs;
using DPManagement.Domain.Enums;

namespace DPManagement.Application.Validators;

public class VinculoCreateUpdateValidator : AbstractValidator<VinculoCreateUpdateDto>
{
    public VinculoCreateUpdateValidator()
    {
        RuleFor(x => x.ColaboradorId)
            .NotEmpty().WithMessage("O colaborador é obrigatório.");

        RuleFor(x => x.OrgaoId)
            .NotEmpty().WithMessage("O órgão de lotação é obrigatório.");

        RuleFor(x => x.CargoId)
            .NotEmpty().WithMessage("O cargo é obrigatório.");

        RuleFor(x => x.Matricula)
            .NotEmpty().WithMessage("A matrícula é obrigatória.")
            .MaximumLength(50).WithMessage("A matrícula não pode exceder 50 caracteres.");

        RuleFor(x => x.RegimeJuridicoId)
            .Must(v => (int)v > 0).WithMessage("O regime jurídico é obrigatório.");

        RuleFor(x => x.FormaIngressoId)
            .Must(v => (int)v > 0).WithMessage("A forma de ingresso é obrigatória.");

        RuleFor(x => x.CentroCustoId)
            .NotEmpty().WithMessage("O centro de custo é obrigatório.");

        RuleFor(x => x.DataAdmissao)
            .NotEmpty().WithMessage("A data de admissão é obrigatória.")
            .LessThanOrEqualTo(DateTime.Now.Date.AddDays(1)).WithMessage("A data de admissão não pode ser futura.");

        RuleFor(x => x.SalarioBase)
            .GreaterThan(0).WithMessage("O salário base deve ser maior que zero.")
            .PrecisionScale(18, 2, true).WithMessage("O salário base excede o limite de 2 casas decimais.");
    }
}
