using DPManagement.Application.DTOs;
using DPManagement.Application.Interfaces;
using DPManagement.Domain.Entities;
using DPManagement.Domain.Enums;
using DPManagement.Domain.Extensions;
using DPManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DPManagement.Infrastructure.Services;

public class RubricaService : IRubricaService
{
    private readonly DPManagementDbContext _context;

    public RubricaService(DPManagementDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResultDto<RubricaDto>> GetPaginatedAsync(int page, int pageSize, string? filtro = null)
    {
        var query = _context.Rubricas.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            query = query.Where(r => r.Codigo.Contains(filtro) || r.Descricao.Contains(filtro));
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderBy(r => r.Codigo)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new RubricaDto
            {
                Id = r.Id,
                Codigo = r.Codigo,
                Descricao = r.Descricao,
                Tipo = r.Tipo,
                TipoDescricao = r.Tipo.GetDescription(),
                IncideIR = r.IncideIR,
                IncidePrevidencia = r.IncidePrevidencia,
                Ativo = r.Ativo
            })
            .ToListAsync();

        return new PagedResultDto<RubricaDto>(items, totalCount, page, pageSize);
    }

    public async Task<IEnumerable<RubricaDto>> GetAllAsync(bool showDeleted = false)
    {
        var query = _context.Rubricas.AsNoTracking();
        if (showDeleted) query = query.IgnoreQueryFilters();

        return await query
            .OrderBy(r => r.Codigo)
            .Select(r => new RubricaDto
            {
                Id = r.Id,
                Codigo = r.Codigo,
                Descricao = r.Descricao,
                Tipo = r.Tipo,
                TipoDescricao = r.Tipo.GetDescription(),
                IncideIR = r.IncideIR,
                IncidePrevidencia = r.IncidePrevidencia,
                Ativo = r.Ativo
            })
            .ToListAsync();
    }

    public async Task<RubricaDto?> GetByIdAsync(Guid id)
    {
        var r = await _context.Rubricas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (r == null) return null;

        return new RubricaDto
        {
            Id = r.Id,
            Codigo = r.Codigo,
            Descricao = r.Descricao,
            Tipo = r.Tipo,
            TipoDescricao = r.Tipo.GetDescription(),
            IncideIR = r.IncideIR,
            IncidePrevidencia = r.IncidePrevidencia,
            Ativo = r.Ativo
        };
    }

    public async Task<RubricaDto> CreateAsync(RubricaCreateUpdateDto dto)
    {
        if (await _context.Rubricas.AnyAsync(r => r.Codigo == dto.Codigo))
        {
            throw new Exception($"Já existe uma rubrica cadastrada com o código {dto.Codigo}.");
        }

        var rubrica = new Rubrica
        {
            Id = Guid.NewGuid(),
            Codigo = dto.Codigo,
            Descricao = dto.Descricao,
            Tipo = dto.Tipo,
            IncideIR = dto.IncideIR,
            IncidePrevidencia = dto.IncidePrevidencia,
            Ativo = true,
            IsDeleted = false
        };

        _context.Rubricas.Add(rubrica);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(rubrica.Id) ?? throw new Exception("Falha ao recuperar rubrica criada.");
    }

    public async Task UpdateAsync(Guid id, RubricaCreateUpdateDto dto)
    {
        var r = await _context.Rubricas.FindAsync(id);
        if (r == null) throw new Exception("Rubrica não encontrada.");

        if (await _context.Rubricas.AnyAsync(rub => rub.Codigo == dto.Codigo && rub.Id != id))
        {
            throw new Exception($"Já existe outra rubrica cadastrada com o código {dto.Codigo}.");
        }

        r.Codigo = dto.Codigo;
        r.Descricao = dto.Descricao;
        r.Tipo = dto.Tipo;
        r.IncideIR = dto.IncideIR;
        r.IncidePrevidencia = dto.IncidePrevidencia;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var r = await _context.Rubricas.FindAsync(id);
        if (r == null) throw new Exception("Rubrica não encontrada.");

        _context.Rubricas.Remove(r);
        await _context.SaveChangesAsync();
    }

    public async Task ToggleStatusAsync(Guid id)
    {
        var r = await _context.Rubricas.FindAsync(id);
        if (r == null) throw new Exception("Rubrica não encontrada.");

        r.Ativo = !r.Ativo;
        await _context.SaveChangesAsync();
    }
}
