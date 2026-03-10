using DPManagement.Application.Interfaces;
using DPManagement.Domain.Entities;
using DPManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DPManagement.Infrastructure.Services;

public class CentroCustoService : ICentroCustoService
{
    private readonly DPManagementDbContext _context;

    public CentroCustoService(DPManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CentroCusto>> ObterTodosAsync(string? descricao = null, Guid? orgaoId = null)
    {
        var query = _context.CentroCustos
            .Include(c => c.Orgao)
            .Where(c => !c.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(descricao))
            query = query.Where(c => c.Descricao.ToLower().Contains(descricao.ToLower()));

        if (orgaoId.HasValue && orgaoId.Value != Guid.Empty)
            query = query.Where(c => c.OrgaoId == orgaoId.Value);

        return await query.OrderBy(c => c.Descricao).ToListAsync();
    }

    public async Task<CentroCusto?> ObterPorIdAsync(Guid id)
    {
        return await _context.CentroCustos
            .Include(c => c.Orgao)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
    }

    public async Task<CentroCusto> AdicionarAsync(CentroCusto centroCusto)
    {
        _context.CentroCustos.Add(centroCusto);
        await _context.SaveChangesAsync();
        return centroCusto;
    }

    public async Task AtualizarAsync(CentroCusto centroCusto)
    {
        _context.Entry(centroCusto).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task AtivarInativarAsync(Guid id, bool ativo)
    {
        var centro = await ObterPorIdAsync(id);
        if (centro != null)
        {
            centro.Ativo = ativo;
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoverAsync(Guid id)
    {
        var centro = await ObterPorIdAsync(id);
        if (centro != null)
        {
            centro.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}
