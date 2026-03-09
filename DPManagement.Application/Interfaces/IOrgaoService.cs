using DPManagement.Domain.Entities;

namespace DPManagement.Application.Interfaces;

public interface IOrgaoService
{
    Task<IEnumerable<Orgao>> ObterTodosAsync(string? nome = null, string? abreviatura = null);
    Task<IEnumerable<Orgao>> ObterPorNivelAsync(int nivel);
    Task<Orgao?> ObterPorIdAsync(Guid id);
    Task<Orgao> AdicionarAsync(Orgao orgao);
    Task AtualizarAsync(Orgao orgao);
    Task RemoverAsync(Guid id);
    Task AlternarStatusAsync(Guid id, bool ativo);
}
