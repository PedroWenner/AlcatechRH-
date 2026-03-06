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

    public async Task<IEnumerable<CargoDto>> GetAllAsync()
    {
        var entities = await _cargoRepository.GetAllAsync();
        return entities.Select(e => new CargoDto { Id = e.Id, Nome = e.Nome, CBO = e.CBO });
    }

    public async Task<PagedResultDto<CargoDto>> GetPagedAsync(int page, int pageSize)
    {
        var result = await _cargoRepository.GetPagedAsync(page, pageSize);
        return new PagedResultDto<CargoDto>(
            result.Items.Select(e => new CargoDto { Id = e.Id, Nome = e.Nome, CBO = e.CBO }),
            result.TotalCount,
            page,
            pageSize
        );
    }

    public async Task<CargoDto?> GetByIdAsync(Guid id)
    {
        var entity = await _cargoRepository.GetByIdAsync(id);
        return entity == null ? null : new CargoDto { Id = entity.Id, Nome = entity.Nome, CBO = entity.CBO };
    }

    public async Task AddAsync(CreateCargoDto cargoDto)
    {
        var entity = new Cargo { Nome = cargoDto.Nome, CBO = cargoDto.CBO };
        await _cargoRepository.AddAsync(entity);
    }

    public async Task UpdateAsync(UpdateCargoDto cargoDto)
    {
        var entity = new Cargo { Id = cargoDto.Id, Nome = cargoDto.Nome, CBO = cargoDto.CBO };
        await _cargoRepository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(Guid id) => await _cargoRepository.DeleteAsync(id);
}
