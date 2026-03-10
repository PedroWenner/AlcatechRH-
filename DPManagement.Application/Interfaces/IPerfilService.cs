using DPManagement.Application.Common;
using DPManagement.Domain.Entities;

namespace DPManagement.Application.Interfaces;

public interface IPerfilService
{
    Task<OperationResult<IEnumerable<Perfil>>> ListarTodosAsync();
    Task<OperationResult<Perfil?>> ObterPorIdAsync(Guid id);
    Task<OperationResult<Perfil>> AdicionarAsync(Perfil perfil);
    Task<OperationResult> AtualizarAsync(Perfil perfil);
    Task<OperationResult> AtivarInativarAsync(Guid id, bool ativo);
    Task<OperationResult> RemoverAsync(Guid id);
    Task<OperationResult> AtribuirPermissoesAsync(Guid perfilId, IEnumerable<Guid> permissaoIds);
}
