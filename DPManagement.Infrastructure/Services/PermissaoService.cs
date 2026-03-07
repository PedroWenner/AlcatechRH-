using DPManagement.Application.Interfaces;
using DPManagement.Domain.Entities;
using DPManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DPManagement.Infrastructure.Services;

public class PermissaoService : IPermissaoService
{
    private readonly DPManagementDbContext _context;

    public PermissaoService(DPManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Permissao>> ListarTodasAsync()
    {
        return await _context.Permissoes
            .OrderBy(p => p.Modulo)
            .ThenBy(p => p.Acao)
            .ToListAsync();
    }

    public async Task<Permissao?> ObterPorIdAsync(Guid id)
    {
        return await _context.Permissoes
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Permissao> AdicionarAsync(Permissao permissao)
    {
        _context.Permissoes.Add(permissao);
        await _context.SaveChangesAsync();
        return permissao;
    }

    public async Task AtualizarAsync(Permissao permissao)
    {
        _context.Entry(permissao).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task AtivarInativarAsync(Guid id, bool ativo)
    {
        var permissao = await ObterPorIdAsync(id);
        if (permissao != null)
        {
            permissao.Ativo = ativo;
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoverAsync(Guid id)
    {
        var permissao = await ObterPorIdAsync(id);
        if (permissao != null)
        {
            permissao.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}
