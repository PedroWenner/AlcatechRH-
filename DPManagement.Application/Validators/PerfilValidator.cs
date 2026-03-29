using DPManagement.Application.DTOs;
using FluentValidation;

namespace DPManagement.Application.Validators;

public class CreatePerfilValidator : AbstractValidator<PerfilRequestDto>
{
    public CreatePerfilValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do perfil é obrigatório.")
            .MaximumLength(100).WithMessage("O nome não pode exceder 100 caracteres.");
    }
}

public class UpdatePerfilValidator : AbstractValidator<PerfilRequestDto>
{
    public UpdatePerfilValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do perfil é obrigatório.")
            .MaximumLength(100).WithMessage("O nome não pode exceder 100 caracteres.");
    }
}
