using FluentValidation;
using DPManagement.Application.DTOs;

namespace DPManagement.Application.Validators;

public class RubricaCreateUpdateValidator : AbstractValidator<RubricaCreateUpdateDto>
{
    public RubricaCreateUpdateValidator()
    {
        RuleFor(x => x.Codigo)
            .NotEmpty().WithMessage("O código é obrigatório.")
            .MaximumLength(20).WithMessage("O código não pode exceder 20 caracteres.");

        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(200).WithMessage("A descrição não pode exceder 200 caracteres.");

        RuleFor(x => x.NatRubr)
            .NotEmpty().WithMessage("A natureza da rubrica (eSocial) é obrigatória.")
            .MaximumLength(10).WithMessage("A natureza não pode exceder 10 caracteres.");

        RuleFor(x => x.IniValid)
            .NotEmpty().WithMessage("O início de validade é obrigatório.")
            .Matches(@"^\d{4}-\d{2}$").WithMessage("O início de validade deve estar no formato MM/AAAA.");

        RuleFor(x => x.FimValid)
            .Matches(@"^\d{4}-\d{2}$").When(x => !string.IsNullOrEmpty(x.FimValid))
            .WithMessage("O fim de validade deve estar no formato MM/AAAA.");

        RuleFor(x => x.IdeTabRubr)
            .NotEmpty().WithMessage("O identificador da tabela é obrigatório.")
            .MaximumLength(30).WithMessage("O identificador da tabela não pode exceder 30 caracteres.");

        RuleFor(x => x.CodIncCP)
            .NotEmpty().WithMessage("A incidência de contribuição previdenciária é obrigatória.")
            .MaximumLength(2).WithMessage("O código de incidência deve ter no máximo 2 caracteres.");

        RuleFor(x => x.CodIncIRRF)
            .NotEmpty().WithMessage("A incidência de IRRF é obrigatória.")
            .MaximumLength(2).WithMessage("O código de incidência deve ter no máximo 2 caracteres.");

        RuleFor(x => x.CodIncFGTS)
            .NotEmpty().WithMessage("A incidência de FGTS é obrigatória.")
            .MaximumLength(2).WithMessage("O código de incidência deve ter no máximo 2 caracteres.");

        RuleFor(x => x.CodIncPisPasep)
            .NotEmpty().WithMessage("A incidência de PIS/PASEP é obrigatória.")
            .MaximumLength(2).WithMessage("O código de incidência deve ter no máximo 2 caracteres.");
    }
}
