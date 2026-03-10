using DPManagement.Domain.Entities;

namespace DPManagement.Application.Interfaces;

public interface IPermissaoService
{
    Task<IEnumerable<Permissao>> ListarTodasAsync();
    Task<Permissao?> ObterPorIdAsync(Guid id);
    Task<Permissao> AdicionarAsync(Permissao permissao);
    Task AtualizarAsync(Permissao permissao);
    Task AtivarInativarAsync(Guid id, bool ativo);
    Task RemoverAsync(Guid id);
}
