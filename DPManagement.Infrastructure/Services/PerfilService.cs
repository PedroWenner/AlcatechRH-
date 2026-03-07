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
            .Include(p => p.PerfilPermissoes)
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

    public async Task<Perfil> AdicionarAsync(Perfil perfil)
    {
        _context.Perfis.Add(perfil);
        await _context.SaveChangesAsync();
        return perfil;
    }

    public async Task AtualizarAsync(Perfil perfil)
    {
        _context.Entry(perfil).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task AtivarInativarAsync(Guid id, bool ativo)
    {
        var perfil = await ObterPorIdAsync(id);
        if (perfil != null)
        {
            perfil.Ativo = ativo;
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoverAsync(Guid id)
    {
        var perfil = await ObterPorIdAsync(id);
        if (perfil != null)
        {
            perfil.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task AtribuirPermissoesAsync(Guid perfilId, IEnumerable<Guid> permissaoIds)
    {
        var perfil = await _context.Perfis
            .Include(p => p.PerfilPermissoes)
            .FirstOrDefaultAsync(p => p.Id == perfilId);

        if (perfil == null) return;

        // Limpa as permissões antigas
        _context.PerfilPermissoes.RemoveRange(perfil.PerfilPermissoes);
        
        // Adiciona as novas
        foreach (var permissaoId in permissaoIds)
        {
            perfil.PerfilPermissoes.Add(new PerfilPermissao
            {
                PerfilId = perfilId,
                PermissaoId = permissaoId
            });
        }

        await _context.SaveChangesAsync();
    }
}
