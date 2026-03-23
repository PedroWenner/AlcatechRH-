using DPManagement.Application.Common;
using DPManagement.Application.DTOs;

namespace DPManagement.Application.Interfaces;

public interface ICentroCustoService
{
    Task<OperationResult<IEnumerable<CentroCustoDto>>> ObterTodosAsync(string? descricao = null, Guid? orgaoId = null);
    Task<OperationResult<CentroCustoDto?>> ObterPorIdAsync(Guid id);
    Task<OperationResult<CentroCustoDto>> AdicionarAsync(CentroCustoRequestDto request);
    Task<OperationResult> AtualizarAsync(CentroCustoRequestDto request);
    Task<OperationResult> RemoverAsync(Guid id);
    Task<OperationResult> AtivarInativarAsync(Guid id, bool ativo);
}
