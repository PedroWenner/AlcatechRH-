using DPManagement.Application.Interfaces;
using DPManagement.Domain.Entities;
using DPManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DPManagement.Infrastructure.Services;

public class PerfilService : IPerfilService
{
    private readonly DPManagementDbContext _context;

    public PerfilService(DPManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Perfil>> ListarTodosAsync()
    {
        return await _context.Perfis
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }

    public async Task<Perfil?> ObterPorIdAsync(Guid id)
    {
        return await _context.Perfis
            .Include(p => p.PerfilPermissoes)
                .ThenInclude(pp => pp.Permissao)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}
