using FluentValidation;
using DPManagement.Application.DTOs;
using DPManagement.Application.Common;

namespace DPManagement.Application.Validators;

public class CreateColaboradorValidator : AbstractValidator<CreateColaboradorDto>
{
    public CreateColaboradorValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(200).WithMessage("O nome não pode exceder 200 caracteres.");

        RuleFor(x => x.CPF)
            .NotEmpty().WithMessage("O CPF é obrigatório.")
            .Must(Validations.ValidarCpf).WithMessage("CPF inválido.");

        RuleFor(x => x.DataNascimento)
            .NotEmpty().WithMessage("A data de nascimento é obrigatória.")
            .LessThan(DateTime.Now.Date).WithMessage("A data de nascimento deve ser anterior a hoje.");

        RuleFor(x => x.CEP)
            .NotEmpty().WithMessage("O CEP é obrigatório.")
            .Length(8).WithMessage("O CEP deve ter 8 dígitos.");

        RuleFor(x => x.Logradouro)
            .NotEmpty().WithMessage("O logradouro é obrigatório.")
            .MaximumLength(200).WithMessage("O logradouro não pode exceder 200 caracteres.");

        RuleFor(x => x.Numero)
            .NotEmpty().WithMessage("O número é obrigatório.")
            .MaximumLength(20).WithMessage("O número não pode exceder 20 caracteres.");

        RuleFor(x => x.Bairro)
            .NotEmpty().WithMessage("O bairro é obrigatório.")
            .MaximumLength(100).WithMessage("O bairro não pode exceder 100 caracteres.");

        RuleFor(x => x.Cidade)
            .NotEmpty().WithMessage("A cidade é obrigatória.")
            .MaximumLength(100).WithMessage("A cidade não pode exceder 100 caracteres.");

        RuleFor(x => x.Estado)
            .NotEmpty().WithMessage("O estado é obrigatório.")
            .Length(2).WithMessage("O estado deve ter 2 caracteres (UF).");

        RuleFor(x => x.CargoId)
            .NotEmpty().WithMessage("O cargo é obrigatório.");
    }
}

public class UpdateColaboradorValidator : AbstractValidator<UpdateColaboradorDto>
{
    public UpdateColaboradorValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O ID do colaborador é obrigatório.");

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(200).WithMessage("O nome não pode exceder 200 caracteres.");

        RuleFor(x => x.CPF)
            .NotEmpty().WithMessage("O CPF é obrigatório.")
            .Must(Validations.ValidarCpf).WithMessage("CPF inválido.");

        RuleFor(x => x.DataNascimento)
            .NotEmpty().WithMessage("A data de nascimento é obrigatória.")
            .LessThan(DateTime.Now.Date).WithMessage("A data de nascimento deve ser anterior a hoje.");

        RuleFor(x => x.CEP)
            .NotEmpty().WithMessage("O CEP é obrigatório.")
            .Length(8).WithMessage("O CEP deve ter 8 dígitos.");

        RuleFor(x => x.Logradouro)
            .NotEmpty().WithMessage("O logradouro é obrigatório.")
            .MaximumLength(200).WithMessage("O logradouro não pode exceder 200 caracteres.");

        RuleFor(x => x.Numero)
            .NotEmpty().WithMessage("O número é obrigatório.")
            .MaximumLength(20).WithMessage("O número não pode exceder 20 caracteres.");

        RuleFor(x => x.Bairro)
            .NotEmpty().WithMessage("O bairro é obrigatório.")
            .MaximumLength(100).WithMessage("O bairro não pode exceder 100 caracteres.");

        RuleFor(x => x.Cidade)
            .NotEmpty().WithMessage("A cidade é obrigatória.")
            .MaximumLength(100).WithMessage("A cidade não pode exceder 100 caracteres.");

        RuleFor(x => x.Estado)
            .NotEmpty().WithMessage("O estado é obrigatório.")
            .Length(2).WithMessage("O estado deve ter 2 caracteres (UF).");

        RuleFor(x => x.CargoId)
            .NotEmpty().WithMessage("O cargo é obrigatório.");
    }
}
