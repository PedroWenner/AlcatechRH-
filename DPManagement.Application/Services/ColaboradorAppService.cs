using DPManagement.Application.DTOs;
using DPManagement.Application.Interfaces;
using DPManagement.Domain.Entities;
using DPManagement.Domain.Repositories;

namespace DPManagement.Application.Services;

public class ColaboradorAppService : IColaboradorAppService
{
    private readonly IColaboradorRepository _colaboradorRepository;

    public ColaboradorAppService(IColaboradorRepository colaboradorRepository)
    {
        _colaboradorRepository = colaboradorRepository;
    }

    public async Task<IEnumerable<ColaboradorDto>> GetAllAsync()
    {
        var entities = await _colaboradorRepository.GetAllAsync();
        return entities.Select(e => MapToDto(e));
    }

    public async Task<ColaboradorDto?> GetByIdAsync(Guid id)
    {
        var entity = await _colaboradorRepository.GetByIdAsync(id);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task AddAsync(CreateColaboradorDto dto)
    {
        var entity = new Colaborador
        {
            Nome = dto.Nome,
            CPF = dto.CPF.Replace(".", "").Replace("-", ""), // Strip mask
            RG = dto.RG,
            PIS = dto.PIS,
            DataNascimento = DateTime.SpecifyKind(dto.DataNascimento, DateTimeKind.Utc),
            Telefone = dto.Telefone,
            Celular = dto.Celular,
            CEP = dto.CEP.Replace("-", ""), // Strip mask
            Logradouro = dto.Logradouro,
            Numero = dto.Numero,
            Complemento = dto.Complemento,
            Bairro = dto.Bairro,
            Cidade = dto.Cidade,
            Estado = dto.Estado,
            CargoId = dto.CargoId
        };
        await _colaboradorRepository.AddAsync(entity);
    }

    public async Task UpdateAsync(UpdateColaboradorDto dto)
    {
        var entity = new Colaborador
        {
            Id = dto.Id,
            Nome = dto.Nome,
            CPF = dto.CPF.Replace(".", "").Replace("-", ""),
            RG = dto.RG,
            PIS = dto.PIS,
            DataNascimento = DateTime.SpecifyKind(dto.DataNascimento, DateTimeKind.Utc),
            Telefone = dto.Telefone,
            Celular = dto.Celular,
            CEP = dto.CEP.Replace("-", ""),
            Logradouro = dto.Logradouro,
            Numero = dto.Numero,
            Complemento = dto.Complemento,
            Bairro = dto.Bairro,
            Cidade = dto.Cidade,
            Estado = dto.Estado,
            CargoId = dto.CargoId
        };
        await _colaboradorRepository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(Guid id) => await _colaboradorRepository.DeleteAsync(id);

    private static ColaboradorDto MapToDto(Colaborador e) => new ColaboradorDto
    {
        Id = e.Id,
        Nome = e.Nome,
        CPF = e.CPF,
        RG = e.RG,
        PIS = e.PIS,
        DataNascimento = e.DataNascimento,
        Telefone = e.Telefone,
        Celular = e.Celular,
        CEP = e.CEP,
        Logradouro = e.Logradouro,
        Numero = e.Numero,
        Complemento = e.Complemento,
        Bairro = e.Bairro,
        Cidade = e.Cidade,
        Estado = e.Estado,
        CargoId = e.CargoId,
        CargoNome = e.Cargo?.Nome ?? string.Empty
    };
}
