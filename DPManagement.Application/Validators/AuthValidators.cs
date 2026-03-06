using FluentValidation;
using DPManagement.Application.DTOs;

namespace DPManagement.Application.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("Formato de e-mail inválido.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("A senha é obrigatória.");
    }
}

public class RegistroUsuarioDtoValidator : AbstractValidator<RegistroUsuarioDto>
{
    public RegistroUsuarioDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(150).WithMessage("O nome não pode exceder 150 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("Formato de e-mail inválido.")
            .MaximumLength(150).WithMessage("O e-mail não pode exceder 150 caracteres.");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(6).WithMessage("A senha deve conter no mínimo 6 caracteres.");

        RuleFor(x => x.Perfil)
            .NotEmpty().WithMessage("O perfil é obrigatório.")
            .Must(perfil => perfil == "Admin" || perfil == "RH" || perfil == "Funcionario")
            .WithMessage("O perfil deve ser 'Admin', 'RH' ou 'Funcionario'.");
    }
}
