using DPManagement.Domain.Entities;

namespace DPManagement.Application.Interfaces;

public interface ICentroCustoService
{
    Task<IEnumerable<CentroCusto>> ObterTodosAsync(string? descricao = null, Guid? orgaoId = null);
    Task<CentroCusto?> ObterPorIdAsync(Guid id);
    Task<CentroCusto> AdicionarAsync(CentroCusto centroCusto);
    Task AtualizarAsync(CentroCusto centroCusto);
    Task RemoverAsync(Guid id);
    Task AtivarInativarAsync(Guid id, bool ativo);
}
