using DPManagement.Domain.Entities;

namespace DPManagement.Application.Interfaces;

public interface IDadoBancarioService
{
    Task<IEnumerable<DadoBancario>> ListarPorColaboradorIdAsync(Guid colaboradorId);
    Task<DadoBancario?> ObterPorIdAsync(Guid id);
    Task<DadoBancario> AdicionarAsync(DadoBancario dadoBancario);
    Task AtualizarAsync(DadoBancario dadoBancario);
    Task RemoverAsync(Guid id);
}
