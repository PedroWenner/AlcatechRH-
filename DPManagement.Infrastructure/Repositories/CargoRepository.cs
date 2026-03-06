using DPManagement.Domain.Entities;
using DPManagement.Domain.Repositories;
using DPManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DPManagement.Infrastructure.Repositories;

public class CargoRepository : ICargoRepository
{
    private readonly DPManagementDbContext _context;

    public CargoRepository(DPManagementDbContext context)
    {
        _context = context;
    }

    public async Task<Cargo?> GetByIdAsync(Guid id) => await _context.Cargos.FindAsync(id);

    public async Task<IEnumerable<Cargo>> GetAllAsync() => await _context.Cargos.ToListAsync();

    public async Task<(IEnumerable<Cargo> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
    {
        var totalCount = await _context.Cargos.CountAsync();
        var items = await _context.Cargos
            .OrderBy(c => c.Nome)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return (items, totalCount);
    }

    public async Task AddAsync(Cargo cargo)
    {
        await _context.Cargos.AddAsync(cargo);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Cargo cargo)
    {
        _context.Cargos.Update(cargo);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var cargo = await GetByIdAsync(id);
        if (cargo != null)
        {
            _context.Cargos.Remove(cargo);
            await _context.SaveChangesAsync();
        }
    }
}
