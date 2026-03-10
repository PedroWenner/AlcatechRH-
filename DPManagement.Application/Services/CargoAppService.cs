using DPManagement.Application.Common;
using DPManagement.Application.DTOs;
using DPManagement.Application.Interfaces;
using DPManagement.Domain.Entities;
using DPManagement.Domain.Repositories;

namespace DPManagement.Application.Services;

public class CargoAppService : ICargoAppService
{
    private readonly ICargoRepository _cargoRepository;

    public CargoAppService(ICargoRepository cargoRepository)
    {
        _cargoRepository = cargoRepository;
    }

    public async Task<OperationResult<IEnumerable<CargoDto>>> GetAllAsync()
    {
        var entities = await _cargoRepository.GetAllAsync();
        var dtos = entities.Select(e => new CargoDto { Id = e.Id, Nome = e.Nome, CBO = e.CBO });
        return OperationResult<IEnumerable<CargoDto>>.Ok(dtos);
    }

    public async Task<OperationResult<PagedResultDto<CargoDto>>> GetPagedAsync(int page, int pageSize, string? nome = null, string? cbo = null)
    {
        var result = await _cargoRepository.GetPagedAsync(page, pageSize, nome, cbo);
        var pagedDto = new PagedResultDto<CargoDto>(
            result.Items.Select(e => new CargoDto { Id = e.Id, Nome = e.Nome, CBO = e.CBO }),
            result.TotalCount,
            page,
            pageSize
        );
        return OperationResult<PagedResultDto<CargoDto>>.Ok(pagedDto);
    }

    public async Task<OperationResult<CargoDto?>> GetByIdAsync(Guid id)
    {
        var entity = await _cargoRepository.GetByIdAsync(id);
        if (entity == null) return OperationResult<CargoDto?>.Failure("Cargo não encontrado.");
        return OperationResult<CargoDto?>.Ok(new CargoDto { Id = entity.Id, Nome = entity.Nome, CBO = entity.CBO });
    }

    public async Task<OperationResult> AddAsync(CreateCargoDto cargoDto)
    {
        var entity = new Cargo { Nome = cargoDto.Nome, CBO = cargoDto.CBO };
        await _cargoRepository.AddAsync(entity);
        return OperationResult.Ok("Cargo criado com sucesso.");
    }

    public async Task<OperationResult> UpdateAsync(UpdateCargoDto cargoDto)
    {
        var entity = new Cargo { Id = cargoDto.Id, Nome = cargoDto.Nome, CBO = cargoDto.CBO };
        await _cargoRepository.UpdateAsync(entity);
        return OperationResult.Ok("Cargo atualizado com sucesso.");
    }

    public async Task<OperationResult> DeleteAsync(Guid id)
    {
        await _cargoRepository.DeleteAsync(id);
        return OperationResult.Ok("Cargo excluído com sucesso.");
    }
}
