using DPManagement.Application.Common;
using DPManagement.Application.DTOs;
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

    public async Task<OperationResult<IEnumerable<CentroCustoDto>>> ObterTodosAsync(string? descricao = null, Guid? orgaoId = null)
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
        
        var dtos = items.Select(c => new CentroCustoDto
        {
            Id = c.Id,
            Descricao = c.Descricao,
            OrgaoId = c.OrgaoId,
            OrgaoNome = c.Orgao?.Nome ?? string.Empty,
            Ativo = c.Ativo
        });

        return OperationResult<IEnumerable<CentroCustoDto>>.Ok(dtos);
    }

    public async Task<OperationResult<CentroCustoDto?>> ObterPorIdAsync(Guid id)
    {
        var item = await _context.CentroCustos
            .Include(c => c.Orgao)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        
        if (item == null) return OperationResult<CentroCustoDto?>.Failure("Centro de custo não encontrado.");

        var dto = new CentroCustoDto
        {
            Id = item.Id,
            Descricao = item.Descricao,
            OrgaoId = item.OrgaoId,
            OrgaoNome = item.Orgao?.Nome ?? string.Empty,
            Ativo = item.Ativo
        };

        return OperationResult<CentroCustoDto?>.Ok(dto);
    }

    public async Task<OperationResult<CentroCustoDto>> AdicionarAsync(CentroCustoRequestDto request)
    {
        var centroCusto = new CentroCusto
        {
            Descricao = request.Descricao,
            OrgaoId = request.OrgaoId,
            Ativo = true
        };

        _context.CentroCustos.Add(centroCusto);
        await _context.SaveChangesAsync();

        var orgao = await _context.Orgaos.FindAsync(request.OrgaoId);

        var dto = new CentroCustoDto
        {
            Id = centroCusto.Id,
            Descricao = centroCusto.Descricao,
            OrgaoId = centroCusto.OrgaoId,
            OrgaoNome = orgao?.Nome ?? string.Empty,
            Ativo = centroCusto.Ativo
        };

        return OperationResult<CentroCustoDto>.Ok(dto, "Centro de custo criado com sucesso.");
    }

    public async Task<OperationResult> AtualizarAsync(CentroCustoRequestDto request)
    {
        if (!request.Id.HasValue) return OperationResult.Failure("ID do centro de custo é obrigatório para atualização.");

        var centroCusto = await _context.CentroCustos.FindAsync(request.Id.Value);
        if (centroCusto == null) return OperationResult.Failure("Centro de custo não encontrado.");

        // Se o órgão está sendo alterado, verificamos se existem vínculos
        if (centroCusto.OrgaoId != request.OrgaoId)
        {
            var possuiVinculos = await _context.Vinculos.AnyAsync(v => v.CentroCustoId == centroCusto.Id && !v.IsDeleted);
            if (possuiVinculos)
            {
                return OperationResult.Failure("Não é possível alterar o órgão deste Centro de Custo pois ele possui vínculos ativos.");
            }
        }

        centroCusto.Descricao = request.Descricao;
        centroCusto.OrgaoId = request.OrgaoId;

        await _context.SaveChangesAsync();
        return OperationResult.Ok("Centro de custo atualizado com sucesso.");
    }

    public async Task<OperationResult> AtivarInativarAsync(Guid id, bool ativo)
    {
        var centro = await _context.CentroCustos.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
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
        var centro = await _context.CentroCustos.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        if (centro == null) return OperationResult.Failure("Centro de custo não encontrado.");

        // Verificando se existem vínculos ativos atrelados a este centro de custo
        var possuiVinculos = await _context.Vinculos.AnyAsync(v => v.CentroCustoId == id && !v.IsDeleted);
        if (possuiVinculos)
        {
            return OperationResult.Failure("Não é possível excluir este Centro de Custo pois ele possui vínculos ativos.");
        }

        centro.IsDeleted = true;
        await _context.SaveChangesAsync();
        return OperationResult.Ok("Centro de custo excluído com sucesso.");
    }
}
