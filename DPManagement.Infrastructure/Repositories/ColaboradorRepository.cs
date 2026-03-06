using DPManagement.Domain.Entities;
using DPManagement.Domain.Repositories;
using DPManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DPManagement.Infrastructure.Repositories;

public class ColaboradorRepository : IColaboradorRepository
{
    private readonly DPManagementDbContext _context;

    public ColaboradorRepository(DPManagementDbContext context)
    {
        _context = context;
    }

    public async Task<Colaborador?> GetByIdAsync(Guid id) => 
        await _context.Colaboradores.Include(c => c.Cargo).FirstOrDefaultAsync(c => c.Id == id);

    public async Task<IEnumerable<Colaborador>> GetAllAsync() => 
        await _context.Colaboradores.Include(c => c.Cargo).ToListAsync();

    public async Task<(IEnumerable<Colaborador> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? nome = null, string? cpf = null, Guid? cargoId = null)
    {
        var query = _context.Colaboradores.Include(c => c.Cargo).AsQueryable();

        if (!string.IsNullOrWhiteSpace(nome))
            query = query.Where(c => c.Nome.ToLower().Contains(nome.ToLower()));

        if (!string.IsNullOrWhiteSpace(cpf))
            query = query.Where(c => c.CPF.Contains(cpf));

        if (cargoId.HasValue && cargoId != Guid.Empty)
            query = query.Where(c => c.CargoId == cargoId.Value);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(c => c.Nome)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return (items, totalCount);
    }

    public async Task AddAsync(Colaborador colaborador)
    {
        await _context.Colaboradores.AddAsync(colaborador);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Colaborador colaborador)
    {
        _context.Colaboradores.Update(colaborador);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var colaborador = await _context.Colaboradores.FindAsync(id);
        if (colaborador != null)
        {
            _context.Colaboradores.Remove(colaborador);
            await _context.SaveChangesAsync();
        }
    }
}
