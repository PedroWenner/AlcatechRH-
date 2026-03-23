using DPManagement.Application.Common;
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

    public async Task<OperationResult<IEnumerable<Orgao>>> ObterTodosAsync(string? nome = null, string? abreviatura = null)
    {
        var query = _context.Orgaos.Include(o => o.OrgaoPai).AsQueryable();

        if (!string.IsNullOrWhiteSpace(nome))
            query = query.Where(o => o.Nome.Contains(nome));

        if (!string.IsNullOrWhiteSpace(abreviatura))
            query = query.Where(o => o.Abreviatura.Contains(abreviatura));

        var items = await query
            .OrderBy(o => o.Nivel)
            .ThenBy(o => o.Nome)
            .ToListAsync();

        return OperationResult<IEnumerable<Orgao>>.Ok(items);
    }

    public async Task<OperationResult<IEnumerable<Orgao>>> ObterPorNivelAsync(int nivel)
    {
        var items = await _context.Orgaos
            .Where(o => o.Nivel == nivel && o.Ativo && !o.IsDeleted)
            .OrderBy(o => o.Nome)
            .ToListAsync();
        return OperationResult<IEnumerable<Orgao>>.Ok(items);
    }

    public async Task<OperationResult<Orgao?>> ObterPorIdAsync(Guid id)
    {
        var item = await _context.Orgaos.FindAsync(id);
        if (item == null) return OperationResult<Orgao?>.Failure("Órgão não encontrado.");
        return OperationResult<Orgao?>.Ok(item);
    }

    public async Task<OperationResult<Orgao>> AdicionarAsync(Orgao orgao)
    {
        _context.Orgaos.Add(orgao);
        await _context.SaveChangesAsync();
        return OperationResult<Orgao>.Ok(orgao, "Órgão criado com sucesso.");
    }

    public async Task<OperationResult> AtualizarAsync(Orgao orgao)
    {
        _context.Entry(orgao).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return OperationResult.Ok("Órgão atualizado com sucesso.");
    }

    public async Task<OperationResult> RemoverAsync(Guid id)
    {
        var orgao = await _context.Orgaos.FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
        if (orgao == null) return OperationResult.Failure("Órgão não encontrado.");

        // Check for active Vinculos in this organ or any descendant
        if (await PossuiVinculosAtivosRecursivoAsync(id))
        {
            return OperationResult.Failure("Não é possível excluir esta estrutura pois ela ou uma subestrutura possui vínculos ativos.");
        }

        await MarcarComoDeletadoRecursivoAsync(id);
        
        await _context.SaveChangesAsync();
        return OperationResult.Ok("Órgão excluído com sucesso.");
    }

    private async Task<bool> PossuiVinculosAtivosRecursivoAsync(Guid id)
    {
        // Check current organ
        var temVinculo = await _context.Vinculos.AnyAsync(v => v.OrgaoId == id && !v.IsDeleted);
        if (temVinculo) return true;

        // Check children
        var children = await _context.Orgaos
            .Where(o => o.OrgaoPaiId == id && !o.IsDeleted)
            .Select(o => o.Id)
            .ToListAsync();

        foreach (var childId in children)
        {
            if (await PossuiVinculosAtivosRecursivoAsync(childId)) return true;
        }

        return false;
    }

    private async Task MarcarComoDeletadoRecursivoAsync(Guid id)
    {
        var orgao = await _context.Orgaos.FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
        if (orgao == null) return;

        orgao.IsDeleted = true;

        var children = await _context.Orgaos
            .Where(o => o.OrgaoPaiId == id && !o.IsDeleted)
            .Select(o => o.Id)
            .ToListAsync();

        foreach (var childId in children)
        {
            await MarcarComoDeletadoRecursivoAsync(childId);
        }
    }

    public async Task<OperationResult<int>> ObterContagemDescendentesAsync(Guid id)
    {
        var total = 0;
        var children = await _context.Orgaos
            .Where(o => o.OrgaoPaiId == id && !o.IsDeleted)
            .Select(o => o.Id)
            .ToListAsync();

        total += children.Count;

        foreach (var childId in children)
        {
            var subResult = await ObterContagemDescendentesAsync(childId);
            if (subResult.Success)
            {
                total += subResult.Data;
            }
        }

        return OperationResult<int>.Ok(total);
    }

    public async Task<OperationResult> AlternarStatusAsync(Guid id, bool ativo)
    {
        var result = await ObterPorIdAsync(id);
        var orgao = result.Data;
        if (orgao != null)
        {
            orgao.Ativo = ativo;
            await _context.SaveChangesAsync();
            return OperationResult.Ok(ativo ? "Órgão ativado com sucesso." : "Órgão inativado com sucesso.");
        }
        return OperationResult.Failure("Órgão não encontrado.");
    }
}
