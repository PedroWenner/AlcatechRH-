using DPManagement.Application.Common;
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

    public async Task<OperationResult<PagedResultDto<VinculoDto>>> GetPaginatedAsync(int page, int pageSize, string? matricula, string? nomeColaborador)
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

        return OperationResult<PagedResultDto<VinculoDto>>.Ok(new PagedResultDto<VinculoDto>(items, totalCount, page, pageSize));
    }

    public async Task<OperationResult<IEnumerable<VinculoDto>>> GetAllAsync(bool showDeleted = false)
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

        var items = await query.Select(v => new VinculoDto
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

        return OperationResult<IEnumerable<VinculoDto>>.Ok(items);
    }

    public async Task<OperationResult<VinculoDto?>> GetByIdAsync(Guid id)
    {
        var v = await _context.Vinculos
            .Include(x => x.Colaborador)
            .Include(x => x.Orgao)
            .Include(x => x.Cargo)
            .Include(x => x.CentroCusto)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (v == null) return OperationResult<VinculoDto?>.Failure("Vínculo não encontrado.");

        var dto = new VinculoDto
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

        return OperationResult<VinculoDto?>.Ok(dto);
    }

    public async Task<OperationResult<VinculoDto>> CreateAsync(VinculoCreateUpdateDto dto)
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
        
        var createdDto = await GetByIdAsync(vinculo.Id);
        return OperationResult<VinculoDto>.Ok(createdDto.Data!);
    }

    public async Task<OperationResult> UpdateAsync(Guid id, VinculoCreateUpdateDto dto)
    {
        var vinculo = await _context.Vinculos.FindAsync(id);
        if (vinculo == null)
            return OperationResult.Failure("Vínculo não encontrado.");

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
        return OperationResult.Ok("Vínculo atualizado com sucesso.");
    }

    public async Task<OperationResult> ToggleStatusAsync(Guid id)
    {
        var vinculo = await _context.Vinculos.FindAsync(id);
        if (vinculo == null)
            return OperationResult.Failure("Vínculo não encontrado.");

        vinculo.Ativo = !vinculo.Ativo;
        await _context.SaveChangesAsync();
        return OperationResult.Ok(vinculo.Ativo ? "Vínculo ativado com sucesso." : "Vínculo inativado com sucesso.");
    }

    public async Task<OperationResult> DeleteAsync(Guid id)
    {
        var vinculo = await _context.Vinculos.FindAsync(id);
        if (vinculo == null)
            return OperationResult.Failure("Vínculo não encontrado.");

        _context.Vinculos.Remove(vinculo);
        await _context.SaveChangesAsync();
        return OperationResult.Ok("Vínculo excluído com sucesso.");
    }
}
