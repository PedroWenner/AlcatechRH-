using DPManagement.Application.Common;
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

    public async Task<OperationResult<IEnumerable<CentroCusto>>> ObterTodosAsync(string? descricao = null, Guid? orgaoId = null)
    {
        var query = _context.CentroCustos
            .Include(c => c.Orgao)
            .Where(c => !c.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(descricao))
            query = query.Where(c => c.Descricao.ToLower().Contains(descricao.ToLower()));

        if (orgaoId.HasValue && orgaoId.Value != Guid.Empty)
            query = query.Where(c => c.OrgaoId == orgaoId.Value);

        var items = await query.OrderBy(c => c.Descricao).ToListAsync();
        return OperationResult<IEnumerable<CentroCusto>>.Ok(items);
    }

    public async Task<OperationResult<CentroCusto?>> ObterPorIdAsync(Guid id)
    {
        var item = await _context.CentroCustos
            .Include(c => c.Orgao)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        
        if (item == null) return OperationResult<CentroCusto?>.Failure("Centro de custo não encontrado.");
        return OperationResult<CentroCusto?>.Ok(item);
    }

    public async Task<OperationResult<CentroCusto>> AdicionarAsync(CentroCusto centroCusto)
    {
        _context.CentroCustos.Add(centroCusto);
        await _context.SaveChangesAsync();
        return OperationResult<CentroCusto>.Ok(centroCusto, "Centro de custo criado com sucesso.");
    }

    public async Task<OperationResult> AtualizarAsync(CentroCusto centroCusto)
    {
        _context.Entry(centroCusto).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return OperationResult.Ok("Centro de custo atualizado com sucesso.");
    }

    public async Task<OperationResult> AtivarInativarAsync(Guid id, bool ativo)
    {
        var result = await ObterPorIdAsync(id);
        var centro = result.Data;
        if (centro != null)
        {
            centro.Ativo = ativo;
            await _context.SaveChangesAsync();
            return OperationResult.Ok(ativo ? "Centro de custo ativado com sucesso." : "Centro de custo inativado com sucesso.");
        }
        return OperationResult.Failure("Centro de custo não encontrado.");
    }

    public async Task<OperationResult> RemoverAsync(Guid id)
    {
        var result = await ObterPorIdAsync(id);
        var centro = result.Data;
        if (centro != null)
        {
            centro.IsDeleted = true;
            await _context.SaveChangesAsync();
            return OperationResult.Ok("Centro de custo excluído com sucesso.");
        }
        return OperationResult.Failure("Centro de custo não encontrado.");
    }
}
