using DPManagement.Domain.Entities;

namespace DPManagement.Domain.Repositories;

public interface IColaboradorRepository
{
    Task<Colaborador?> GetByIdAsync(Guid id);
    Task<IEnumerable<Colaborador>> GetAllAsync();
    Task<(IEnumerable<Colaborador> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
    Task AddAsync(Colaborador colaborador);
    Task UpdateAsync(Colaborador colaborador);
    Task DeleteAsync(Guid id);
}
