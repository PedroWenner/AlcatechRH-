using DPManagement.Application.Common;
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

    public async Task<OperationResult<IEnumerable<Permissao>>> ListarTodasAsync()
    {
        var items = await _context.Permissoes
            .OrderBy(p => p.Modulo)
            .ThenBy(p => p.Acao)
            .ToListAsync();
        return OperationResult<IEnumerable<Permissao>>.Ok(items);
    }

    public async Task<OperationResult<Permissao?>> ObterPorIdAsync(Guid id)
    {
        var item = await _context.Permissoes
            .FirstOrDefaultAsync(p => p.Id == id);
        if (item == null) return OperationResult<Permissao?>.Failure("Permissão não encontrada.");
        return OperationResult<Permissao?>.Ok(item);
    }

    public async Task<OperationResult<Permissao>> AdicionarAsync(Permissao permissao)
    {
        _context.Permissoes.Add(permissao);
        await _context.SaveChangesAsync();
        return OperationResult<Permissao>.Ok(permissao, "Permissão criada com sucesso.");
    }

    public async Task<OperationResult> AtualizarAsync(Permissao permissao)
    {
        _context.Entry(permissao).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return OperationResult.Ok("Permissão atualizada com sucesso.");
    }

    public async Task<OperationResult> AtivarInativarAsync(Guid id, bool ativo)
    {
        var result = await ObterPorIdAsync(id);
        var permissao = result.Data;
        if (permissao != null)
        {
            permissao.Ativo = ativo;
            await _context.SaveChangesAsync();
            return OperationResult.Ok(ativo ? "Permissão ativada com sucesso." : "Permissão inativada com sucesso.");
        }
        return OperationResult.Failure("Permissão não encontrada.");
    }

    public async Task<OperationResult> RemoverAsync(Guid id)
    {
        var result = await ObterPorIdAsync(id);
        var permissao = result.Data;
        if (permissao != null)
        {
            permissao.IsDeleted = true;
            await _context.SaveChangesAsync();
            return OperationResult.Ok("Permissão excluída com sucesso.");
        }
        return OperationResult.Failure("Permissão não encontrada.");
    }
}
