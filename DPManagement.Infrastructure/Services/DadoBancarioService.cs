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

    public async Task<IEnumerable<DadoBancario>> ListarPorColaboradorIdAsync(Guid colaboradorId)
    {
        return await _context.DadosBancarios
            .Include(db => db.Banco)
            .Where(db => db.ColaboradorId == colaboradorId)
            .OrderBy(db => db.CodigoBanco)
            .ToListAsync();
    }

    public async Task<DadoBancario?> ObterPorIdAsync(Guid id)
    {
        return await _context.DadosBancarios.FindAsync(id);
    }

    public async Task<DadoBancario> AdicionarAsync(DadoBancario dadoBancario)
    {
        _context.DadosBancarios.Add(dadoBancario);
        await _context.SaveChangesAsync();
        return dadoBancario;
    }

    public async Task AtualizarAsync(DadoBancario dadoBancario)
    {
        _context.Entry(dadoBancario).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Guid id)
    {
        var dadoBancario = await ObterPorIdAsync(id);
        if (dadoBancario != null)
        {
            dadoBancario.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}
