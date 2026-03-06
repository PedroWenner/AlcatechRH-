using DPManagement.Application.DTOs;

namespace DPManagement.Application.Interfaces;

public interface ICargoAppService
{
    Task<IEnumerable<CargoDto>> GetAllAsync();
    Task<CargoDto?> GetByIdAsync(Guid id);
    Task AddAsync(CreateCargoDto cargoDto);
    Task UpdateAsync(UpdateCargoDto cargoDto);
    Task DeleteAsync(Guid id);
}
