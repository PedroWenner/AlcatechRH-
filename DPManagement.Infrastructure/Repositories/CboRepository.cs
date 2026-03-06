using DPManagement.Domain.Entities;
using DPManagement.Domain.Repositories;
using DPManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DPManagement.Infrastructure.Repositories;

public class CboRepository : ICboRepository
{
    private readonly DPManagementDbContext _context;

    public CboRepository(DPManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Cbo>> SearchAsync(string term, int limit = 20)
    {
        if (string.IsNullOrWhiteSpace(term)) return new List<Cbo>();

        term = term.ToLower();

        return await _context.Cbos
            .Where(c => c.Codigo.Contains(term) || c.Titulo.ToLower().Contains(term))
            .Take(limit)
            .ToListAsync();
    }

    public async Task<Cbo?> GetByCodigoAsync(string codigo)
    {
        return await _context.Cbos.FirstOrDefaultAsync(c => c.Codigo == codigo);
    }
}
