using DPManagement.Application.Common;
using DPManagement.Application.DTOs;
using DPManagement.Application.Interfaces;
using DPManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DPManagement.Infrastructure.Services;

public class BancoService : IBancoService
{
    private readonly DPManagementDbContext _context;

    public BancoService(DPManagementDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<IEnumerable<BancoDto>>> SearchAsync(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return OperationResult<IEnumerable<BancoDto>>.Ok(Enumerable.Empty<BancoDto>());

        term = term.ToLower();
        var bancos = await _context.Bancos
            .Where(b => b.CodigoBanco.Contains(term) || b.Nome.ToLower().Contains(term) || b.NomeCurto.ToLower().Contains(term))
            .OrderBy(b => b.CodigoBanco)
            .Take(50)
            .ToListAsync();

        return OperationResult<IEnumerable<BancoDto>>.Ok(bancos.Select(b => new BancoDto
        {
            Id = b.Id,
            Codigo = b.CodigoBanco,
            Titulo = b.Nome
        }));
    }

    public async Task<OperationResult<BancoDto>> GetByCodigoAsync(string codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo)) return OperationResult<BancoDto>.Failure("Código do banco não informado.");

        var b = await _context.Bancos.FirstOrDefaultAsync(x => x.CodigoBanco == codigo);
        if (b == null) return OperationResult<BancoDto>.Failure("Banco não encontrado.");

        return OperationResult<BancoDto>.Ok(new BancoDto
        {
            Id = b.Id,
            Codigo = b.CodigoBanco,
            Titulo = b.Nome
        });
    }
}
