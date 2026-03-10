using DPManagement.Application.Interfaces;
using DPManagement.Domain.Entities;
using DPManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DPManagement.Infrastructure.Services;

public class OrgaoService : IOrgaoService
{
    private readonly DPManagementDbContext _context;

    public OrgaoService(DPManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Orgao>> ObterTodosAsync(string? nome = null, string? abreviatura = null)
    {
        var query = _context.Orgaos.Include(o => o.OrgaoPai).AsQueryable();

        if (!string.IsNullOrWhiteSpace(nome))
            query = query.Where(o => o.Nome.Contains(nome));

        if (!string.IsNullOrWhiteSpace(abreviatura))
            query = query.Where(o => o.Abreviatura.Contains(abreviatura));

        return await query
            .OrderBy(o => o.Nivel)
            .ThenBy(o => o.Nome)
            .ToListAsync();
    }

    public async Task<IEnumerable<Orgao>> ObterPorNivelAsync(int nivel)
    {
        return await _context.Orgaos
            .Where(o => o.Nivel == nivel && o.Ativo && !o.IsDeleted)
            .OrderBy(o => o.Nome)
            .ToListAsync();
    }

    public async Task<Orgao?> ObterPorIdAsync(Guid id)
    {
        return await _context.Orgaos.FindAsync(id);
    }

    public async Task<Orgao> AdicionarAsync(Orgao orgao)
    {
        _context.Orgaos.Add(orgao);
        await _context.SaveChangesAsync();
        return orgao;
    }

    public async Task AtualizarAsync(Orgao orgao)
    {
        _context.Entry(orgao).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Guid id)
    {
        var orgao = await ObterPorIdAsync(id);
        if (orgao != null)
        {
            orgao.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task AlternarStatusAsync(Guid id, bool ativo)
    {
        var orgao = await ObterPorIdAsync(id);
        if (orgao != null)
        {
            orgao.Ativo = ativo;
            await _context.SaveChangesAsync();
        }
    }
}
