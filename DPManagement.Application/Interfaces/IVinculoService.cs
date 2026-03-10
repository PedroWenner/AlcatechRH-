using DPManagement.Application.Common;
using DPManagement.Application.DTOs;

namespace DPManagement.Application.Interfaces;

public interface IVinculoService
{
    Task<OperationResult<PagedResultDto<VinculoDto>>> GetPaginatedAsync(int page, int pageSize, string? matricula, string? nomeColaborador);
    Task<OperationResult<IEnumerable<VinculoDto>>> GetAllAsync(bool showDeleted = false);
    Task<OperationResult<VinculoDto?>> GetByIdAsync(Guid id);
    Task<OperationResult<VinculoDto>> CreateAsync(VinculoCreateUpdateDto dto);
    Task<OperationResult> UpdateAsync(Guid id, VinculoCreateUpdateDto dto);
    Task<OperationResult> ToggleStatusAsync(Guid id);
    Task<OperationResult> DeleteAsync(Guid id);
}
