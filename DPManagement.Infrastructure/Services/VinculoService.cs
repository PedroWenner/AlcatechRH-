using DPManagement.Application.DTOs;
using DPManagement.Application.Interfaces;
using DPManagement.Domain.Entities;
using DPManagement.Domain.Extensions;
using DPManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DPManagement.Infrastructure.Services;

public class VinculoService : IVinculoService
{
    private readonly DPManagementDbContext _context;

    public VinculoService(DPManagementDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResultDto<VinculoDto>> GetPaginatedAsync(int page, int pageSize, string? matricula, string? nomeColaborador)
    {
        var query = _context.Vinculos
            .Include(v => v.Colaborador)
            .Include(v => v.Orgao)
            .Include(v => v.Cargo)
            .Include(v => v.CentroCusto)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(matricula))
        {
            query = query.Where(v => v.Matricula.Contains(matricula));
        }

        if (!string.IsNullOrWhiteSpace(nomeColaborador))
        {
            query = query.Where(v => v.Colaborador.Nome.ToLower().Contains(nomeColaborador.ToLower()));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(v => v.DataAdmissao)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(v => new VinculoDto
            {
                Id = v.Id,
                ColaboradorId = v.ColaboradorId,
                ColaboradorNome = v.Colaborador.Nome,
                ColaboradorCpf = v.Colaborador.CPF,
                OrgaoId = v.OrgaoId,
                OrgaoNome = v.Orgao.Nome,
                OrgaoAbreviatura = v.Orgao.Abreviatura,
                Matricula = v.Matricula,
                CargoId = v.CargoId,
                CargoNome = v.Cargo.Nome,
                RegimeJuridicoId = v.RegimeJuridicoId,
                RegimeJuridicoDescricao = v.RegimeJuridicoId.GetDescription(),
                FormaIngressoId = v.FormaIngressoId,
                FormaIngressoDescricao = v.FormaIngressoId.GetDescription(),
                CentroCustoId = v.CentroCustoId,
                CentroCustoDescricao = v.CentroCusto.Descricao,
                DataAdmissao = v.DataAdmissao,
                Ativo = v.Ativo
            })
            .ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResultDto<VinculoDto>(items, totalCount, page, pageSize);
    }

    public async Task<IEnumerable<VinculoDto>> GetAllAsync(bool showDeleted = false)
    {
        var query = _context.Vinculos
            .Include(v => v.Colaborador)
            .Include(v => v.Orgao)
            .Include(v => v.Cargo)
            .Include(v => v.CentroCusto)
            .AsNoTracking();

        if (showDeleted)
        {
            query = query.IgnoreQueryFilters();
        }

        return await query.Select(v => new VinculoDto
        {
            Id = v.Id,
            ColaboradorId = v.ColaboradorId,
            ColaboradorNome = v.Colaborador.Nome,
            ColaboradorCpf = v.Colaborador.CPF,
            OrgaoId = v.OrgaoId,
            OrgaoNome = v.Orgao.Nome,
            OrgaoAbreviatura = v.Orgao.Abreviatura,
            Matricula = v.Matricula,
            CargoId = v.CargoId,
            CargoNome = v.Cargo.Nome,
            RegimeJuridicoId = v.RegimeJuridicoId,
            RegimeJuridicoDescricao = v.RegimeJuridicoId.GetDescription(),
            FormaIngressoId = v.FormaIngressoId,
            FormaIngressoDescricao = v.FormaIngressoId.GetDescription(),
            CentroCustoId = v.CentroCustoId,
            CentroCustoDescricao = v.CentroCusto.Descricao,
            DataAdmissao = v.DataAdmissao,
            Ativo = v.Ativo
        }).ToListAsync();
    }

    public async Task<VinculoDto?> GetByIdAsync(Guid id)
    {
        var v = await _context.Vinculos
            .Include(x => x.Colaborador)
            .Include(x => x.Orgao)
            .Include(x => x.Cargo)
            .Include(x => x.CentroCusto)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (v == null) return null;

        return new VinculoDto
        {
            Id = v.Id,
            ColaboradorId = v.ColaboradorId,
            ColaboradorNome = v.Colaborador.Nome,
            ColaboradorCpf = v.Colaborador.CPF,
            OrgaoId = v.OrgaoId,
            OrgaoNome = v.Orgao.Nome,
            OrgaoAbreviatura = v.Orgao.Abreviatura,
            Matricula = v.Matricula,
            CargoId = v.CargoId,
            CargoNome = v.Cargo.Nome,
            RegimeJuridicoId = v.RegimeJuridicoId,
            RegimeJuridicoDescricao = v.RegimeJuridicoId.GetDescription(),
            FormaIngressoId = v.FormaIngressoId,
            FormaIngressoDescricao = v.FormaIngressoId.GetDescription(),
            CentroCustoId = v.CentroCustoId,
            CentroCustoDescricao = v.CentroCusto.Descricao,
            DataAdmissao = v.DataAdmissao,
            Ativo = v.Ativo
        };
    }

    public async Task<VinculoDto> CreateAsync(VinculoCreateUpdateDto dto)
    {
        var vinculo = new Vinculo
        {
            Id = Guid.NewGuid(),
            ColaboradorId = dto.ColaboradorId,
            OrgaoId = dto.OrgaoId,
            Matricula = dto.Matricula,
            CargoId = dto.CargoId,
            RegimeJuridicoId = dto.RegimeJuridicoId,
            FormaIngressoId = dto.FormaIngressoId,
            CentroCustoId = dto.CentroCustoId,
            DataAdmissao = dto.DataAdmissao,
            Ativo = true,
            IsDeleted = false
        };

        _context.Vinculos.Add(vinculo);
        await _context.SaveChangesAsync();
        
        return await GetByIdAsync(vinculo.Id) ?? throw new Exception("Falha ao recuperar víncuo recém criado");
    }

    public async Task UpdateAsync(Guid id, VinculoCreateUpdateDto dto)
    {
        var vinculo = await _context.Vinculos.FindAsync(id);
        if (vinculo == null)
            throw new Exception("Vínculo não encontrado.");

        vinculo.ColaboradorId = dto.ColaboradorId;
        vinculo.OrgaoId = dto.OrgaoId;
        vinculo.Matricula = dto.Matricula;
        vinculo.CargoId = dto.CargoId;
        vinculo.RegimeJuridicoId = dto.RegimeJuridicoId;
        vinculo.FormaIngressoId = dto.FormaIngressoId;
        vinculo.CentroCustoId = dto.CentroCustoId;
        vinculo.DataAdmissao = dto.DataAdmissao;

        _context.Vinculos.Update(vinculo);
        await _context.SaveChangesAsync();
    }

    public async Task ToggleStatusAsync(Guid id)
    {
        var vinculo = await _context.Vinculos.FindAsync(id);
        if (vinculo == null)
            throw new Exception("Vínculo não encontrado.");

        vinculo.Ativo = !vinculo.Ativo;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var vinculo = await _context.Vinculos.FindAsync(id);
        if (vinculo == null)
            throw new Exception("Vínculo não encontrado.");

        _context.Vinculos.Remove(vinculo);
        await _context.SaveChangesAsync();
    }
}
