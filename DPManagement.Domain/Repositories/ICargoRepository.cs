using DPManagement.Domain.Entities;

namespace DPManagement.Domain.Repositories;

public interface ICargoRepository
{
    Task<Cargo?> GetByIdAsync(Guid id);
    Task<IEnumerable<Cargo>> GetAllAsync();
    Task AddAsync(Cargo cargo);
    Task UpdateAsync(Cargo cargo);
    Task DeleteAsync(Guid id);
}
