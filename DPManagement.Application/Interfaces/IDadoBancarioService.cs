using DPManagement.Application.Common;
using DPManagement.Domain.Entities;

namespace DPManagement.Application.Interfaces;

public interface IDadoBancarioService
{
    Task<OperationResult<IEnumerable<DadoBancario>>> ListarPorColaboradorIdAsync(Guid colaboradorId);
    Task<OperationResult<DadoBancario>> ObterPorIdAsync(Guid id);
    Task<OperationResult<DadoBancario>> AdicionarAsync(DadoBancario dadoBancario);
    Task<OperationResult> AtualizarAsync(DadoBancario dadoBancario);
    Task<OperationResult> RemoverAsync(Guid id);
    Task<OperationResult> AlternarStatusAsync(Guid id, bool ativo);
}
