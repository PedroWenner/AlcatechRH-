using DPManagement.Application.Common;
using DPManagement.Domain.Entities;

namespace DPManagement.Application.Interfaces;

public interface IOrgaoService
{
    Task<OperationResult<IEnumerable<Orgao>>> ObterTodosAsync(string? nome = null, string? abreviatura = null);
    Task<OperationResult<IEnumerable<Orgao>>> ObterPorNivelAsync(int nivel);
    Task<OperationResult<Orgao?>> ObterPorIdAsync(Guid id);
    Task<OperationResult<Orgao>> AdicionarAsync(Orgao orgao);
    Task<OperationResult> AtualizarAsync(Orgao orgao);
    Task<OperationResult> RemoverAsync(Guid id);
    Task<OperationResult> AlternarStatusAsync(Guid id, bool ativo);
}
