using DPManagement.Domain.Entities;

namespace DPManagement.Application.Interfaces;

public interface IPerfilService
{
    Task<IEnumerable<Perfil>> ListarTodosAsync();
    Task<Perfil?> ObterPorIdAsync(Guid id);
    Task<Perfil> AdicionarAsync(Perfil perfil);
    Task AtualizarAsync(Perfil perfil);
    Task AtivarInativarAsync(Guid id, bool ativo);
    Task RemoverAsync(Guid id);
    Task AtribuirPermissoesAsync(Guid perfilId, IEnumerable<Guid> permissaoIds);
}
