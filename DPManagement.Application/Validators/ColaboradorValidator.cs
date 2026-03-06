using FluentValidation;
using DPManagement.Application.DTOs;
using DPManagement.Application.Common;

namespace DPManagement.Application.Validators;

public class CreateColaboradorValidator : AbstractValidator<CreateColaboradorDto>
{
    public CreateColaboradorValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CPF).NotEmpty().Must(Validations.ValidarCpf).WithMessage("CPF Inválido");
        RuleFor(x => x.DataNascimento).NotEmpty().LessThan(DateTime.Now);
        RuleFor(x => x.CEP).NotEmpty().MaximumLength(8);
        RuleFor(x => x.Logradouro).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Numero).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Bairro).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Cidade).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Estado).NotEmpty().Length(2);
        RuleFor(x => x.CargoId).NotEmpty();
    }
}

public class UpdateColaboradorValidator : AbstractValidator<UpdateColaboradorDto>
{
    public UpdateColaboradorValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CPF).NotEmpty().Must(Validations.ValidarCpf).WithMessage("CPF Inválido");
        RuleFor(x => x.DataNascimento).NotEmpty().LessThan(DateTime.Now);
        RuleFor(x => x.CEP).NotEmpty().MaximumLength(8);
        RuleFor(x => x.Logradouro).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Numero).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Bairro).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Cidade).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Estado).NotEmpty().Length(2);
        RuleFor(x => x.CargoId).NotEmpty();
    }
}
