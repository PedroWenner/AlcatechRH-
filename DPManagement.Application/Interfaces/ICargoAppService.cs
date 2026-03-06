using DPManagement.Application.DTOs;

namespace DPManagement.Application.Interfaces;

public interface ICargoAppService
{
    Task<IEnumerable<CargoDto>> GetAllAsync();
    Task<PagedResultDto<CargoDto>> GetPagedAsync(int page, int pageSize, string? nome = null, string? cbo = null);
    Task<CargoDto?> GetByIdAsync(Guid id);
    Task AddAsync(CreateCargoDto cargoDto);
    Task UpdateAsync(UpdateCargoDto cargoDto);
    Task DeleteAsync(Guid id);
}
