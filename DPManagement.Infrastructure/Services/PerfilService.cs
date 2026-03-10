using DPManagement.Application.Common;
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

    public async Task<OperationResult<IEnumerable<Perfil>>> ListarTodosAsync()
    {
        var items = await _context.Perfis
            .Include(p => p.PerfilPermissoes)
            .OrderBy(p => p.Nome)
            .ToListAsync();
        return OperationResult<IEnumerable<Perfil>>.Ok(items);
    }

    public async Task<OperationResult<Perfil?>> ObterPorIdAsync(Guid id)
    {
        var item = await _context.Perfis
            .Include(p => p.PerfilPermissoes)
                .ThenInclude(pp => pp.Permissao)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (item == null) return OperationResult<Perfil?>.Failure("Perfil não encontrado.");
        return OperationResult<Perfil?>.Ok(item);
    }

    public async Task<OperationResult<Perfil>> AdicionarAsync(Perfil perfil)
    {
        _context.Perfis.Add(perfil);
        await _context.SaveChangesAsync();
        return OperationResult<Perfil>.Ok(perfil, "Perfil criado com sucesso.");
    }

    public async Task<OperationResult> AtualizarAsync(Perfil perfil)
    {
        _context.Entry(perfil).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return OperationResult.Ok("Perfil atualizado com sucesso.");
    }

    public async Task<OperationResult> AtivarInativarAsync(Guid id, bool ativo)
    {
        var result = await ObterPorIdAsync(id);
        var perfil = result.Data;
        if (perfil != null)
        {
            perfil.Ativo = ativo;
            await _context.SaveChangesAsync();
            return OperationResult.Ok(ativo ? "Perfil ativado com sucesso." : "Perfil inativado com sucesso.");
        }
        return OperationResult.Failure("Perfil não encontrado.");
    }

    public async Task<OperationResult> RemoverAsync(Guid id)
    {
        var result = await ObterPorIdAsync(id);
        var perfil = result.Data;
        if (perfil != null)
        {
            perfil.IsDeleted = true;
            await _context.SaveChangesAsync();
            return OperationResult.Ok("Perfil excluído com sucesso.");
        }
        return OperationResult.Failure("Perfil não encontrado.");
    }

    public async Task<OperationResult> AtribuirPermissoesAsync(Guid perfilId, IEnumerable<Guid> permissaoIds)
    {
        var perfil = await _context.Perfis
            .Include(p => p.PerfilPermissoes)
            .FirstOrDefaultAsync(p => p.Id == perfilId);

        if (perfil == null) return OperationResult.Failure("Perfil não encontrado.");

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
        return OperationResult.Ok("Permissões atribuídas com sucesso.");
    }
}
