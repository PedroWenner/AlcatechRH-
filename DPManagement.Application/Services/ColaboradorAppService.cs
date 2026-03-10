using DPManagement.Application.Common;
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

    public async Task<OperationResult<IEnumerable<ColaboradorDto>>> GetAllAsync()
    {
        var entities = await _colaboradorRepository.GetAllAsync();
        var dtos = entities.Select(e => MapToDto(e));
        return OperationResult<IEnumerable<ColaboradorDto>>.Ok(dtos);
    }

    public async Task<OperationResult<PagedResultDto<ColaboradorDto>>> GetPagedAsync(int page, int pageSize, string? nome = null, string? cpf = null, Guid? cargoId = null)
    {
        var result = await _colaboradorRepository.GetPagedAsync(page, pageSize, nome, cpf, cargoId);
        var pagedDto = new PagedResultDto<ColaboradorDto>(
            result.Items.Select(e => MapToDto(e)),
            result.TotalCount,
            page,
            pageSize
        );
        return OperationResult<PagedResultDto<ColaboradorDto>>.Ok(pagedDto);
    }

    public async Task<OperationResult<ColaboradorDto?>> GetByIdAsync(Guid id)
    {
        var entity = await _colaboradorRepository.GetByIdAsync(id);
        if (entity == null) return OperationResult<ColaboradorDto?>.Failure("Colaborador não encontrado.");
        return OperationResult<ColaboradorDto?>.Ok(MapToDto(entity));
    }

    public async Task<OperationResult> AddAsync(CreateColaboradorDto dto)
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
        return OperationResult.Ok("Colaborador criado com sucesso.");
    }

    public async Task<OperationResult> UpdateAsync(UpdateColaboradorDto dto)
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
        return OperationResult.Ok("Colaborador atualizado com sucesso.");
    }

    public async Task<OperationResult> DeleteAsync(Guid id)
    {
        await _colaboradorRepository.DeleteAsync(id);
        return OperationResult.Ok("Colaborador excluído com sucesso.");
    }

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
