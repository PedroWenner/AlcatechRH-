using DPManagement.Application.Common;
using DPManagement.Domain.Entities;

namespace DPManagement.Application.Interfaces;

public interface ICentroCustoService
{
    Task<OperationResult<IEnumerable<CentroCusto>>> ObterTodosAsync(string? descricao = null, Guid? orgaoId = null);
    Task<OperationResult<CentroCusto?>> ObterPorIdAsync(Guid id);
    Task<OperationResult<CentroCusto>> AdicionarAsync(CentroCusto centroCusto);
    Task<OperationResult> AtualizarAsync(CentroCusto centroCusto);
    Task<OperationResult> RemoverAsync(Guid id);
    Task<OperationResult> AtivarInativarAsync(Guid id, bool ativo);
}
