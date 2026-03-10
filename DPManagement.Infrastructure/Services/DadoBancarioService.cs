using DPManagement.Application.Common;
using DPManagement.Application.Interfaces;
using DPManagement.Domain.Entities;
using DPManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DPManagement.Infrastructure.Services;

public class DadoBancarioService : IDadoBancarioService
{
    private readonly DPManagementDbContext _context;

    public DadoBancarioService(DPManagementDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<IEnumerable<DadoBancario>>> ListarPorColaboradorIdAsync(Guid colaboradorId)
    {
        var result = await _context.DadosBancarios
            .Include(db => db.Banco)
            .Where(db => db.ColaboradorId == colaboradorId)
            .OrderBy(db => db.CodigoBanco)
            .ToListAsync();
        
        return OperationResult<IEnumerable<DadoBancario>>.Ok(result);
    }

    public async Task<OperationResult<DadoBancario>> ObterPorIdAsync(Guid id)
    {
        var result = await _context.DadosBancarios.FindAsync(id);
        if (result == null) return OperationResult<DadoBancario>.Failure("Dado bancário não encontrado.");
        return OperationResult<DadoBancario>.Ok(result);
    }

    public async Task<OperationResult<DadoBancario>> AdicionarAsync(DadoBancario dadoBancario)
    {
        _context.DadosBancarios.Add(dadoBancario);
        await _context.SaveChangesAsync();
        return OperationResult<DadoBancario>.Ok(dadoBancario, "Dado bancário adicionado com sucesso.");
    }

    public async Task<OperationResult> AtualizarAsync(DadoBancario dadoBancario)
    {
        _context.Entry(dadoBancario).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return OperationResult.Ok("Dado bancário atualizado com sucesso.");
    }

    public async Task<OperationResult> RemoverAsync(Guid id)
    {
        var result = await ObterPorIdAsync(id);
        if (result.Success)
        {
            result.Data!.IsDeleted = true;
            await _context.SaveChangesAsync();
            return OperationResult.Ok("Dado bancário removido com sucesso.");
        }
        return OperationResult.Failure("Dado bancário não encontrado.");
    }

    public async Task<OperationResult> AlternarStatusAsync(Guid id, bool ativo)
    {
        var result = await ObterPorIdAsync(id);
        if (result.Success)
        {
            result.Data!.Ativo = ativo;
            await _context.SaveChangesAsync();
            return OperationResult.Ok($"Dado bancário {(ativo ? "ativado" : "inativado")} com sucesso.");
        }
        return OperationResult.Failure("Dado bancário não encontrado.");
    }
}
