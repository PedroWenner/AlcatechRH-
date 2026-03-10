using DPManagement.Application.Common;
using DPManagement.Domain.Entities;

namespace DPManagement.Application.Interfaces;

public interface IPermissaoService
{
    Task<OperationResult<IEnumerable<Permissao>>> ListarTodasAsync();
    Task<OperationResult<Permissao?>> ObterPorIdAsync(Guid id);
    Task<OperationResult<Permissao>> AdicionarAsync(Permissao permissao);
    Task<OperationResult> AtualizarAsync(Permissao permissao);
    Task<OperationResult> AtivarInativarAsync(Guid id, bool ativo);
    Task<OperationResult> RemoverAsync(Guid id);
}
