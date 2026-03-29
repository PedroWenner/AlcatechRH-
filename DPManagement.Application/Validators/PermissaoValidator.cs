using DPManagement.Application.DTOs;
using FluentValidation;

namespace DPManagement.Application.Validators;

public class CreatePermissaoValidator : AbstractValidator<PermissaoRequestDto>
{
    public CreatePermissaoValidator()
    {
        RuleFor(x => x.Modulo)
            .NotEmpty().WithMessage("O módulo é obrigatório.")
            .MaximumLength(100).WithMessage("O módulo não pode exceder 100 caracteres.");

        RuleFor(x => x.ModuloPai)
            .MaximumLength(100).WithMessage("O módulo pai não pode exceder 100 caracteres.");

        RuleFor(x => x.Acao)
            .NotEmpty().WithMessage("A ação é obrigatória.")
            .MaximumLength(50).WithMessage("A ação não pode exceder 50 caracteres.");

        RuleFor(x => x.Descricao)
            .MaximumLength(250).WithMessage("A descrição não pode exceder 250 caracteres.");
    }
}

public class UpdatePermissaoValidator : AbstractValidator<PermissaoRequestDto>
{
    public UpdatePermissaoValidator()
    {
        RuleFor(x => x.Modulo)
            .NotEmpty().WithMessage("O módulo é obrigatório.")
            .MaximumLength(100).WithMessage("O módulo não pode exceder 100 caracteres.");

        RuleFor(x => x.ModuloPai)
            .MaximumLength(100).WithMessage("O módulo pai não pode exceder 100 caracteres.");

        RuleFor(x => x.Acao)
            .NotEmpty().WithMessage("A ação é obrigatória.")
            .MaximumLength(50).WithMessage("A ação não pode exceder 50 caracteres.");

        RuleFor(x => x.Descricao)
            .MaximumLength(250).WithMessage("A descrição não pode exceder 250 caracteres.");
    }
}
