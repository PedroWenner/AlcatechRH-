using DPManagement.Domain.Entities;

namespace DPManagement.Domain.Repositories;

public interface ICboRepository
{
    Task<IEnumerable<Cbo>> SearchAsync(string term, int limit = 20);
    Task<Cbo?> GetByCodigoAsync(string codigo);
}
