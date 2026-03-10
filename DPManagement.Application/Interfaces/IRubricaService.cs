using DPManagement.Application.Common;
using DPManagement.Application.DTOs;

namespace DPManagement.Application.Interfaces;

public interface IRubricaService
{
    Task<OperationResult<PagedResultDto<RubricaDto>>> GetPaginatedAsync(int page, int pageSize, string? filtro = null);
    Task<OperationResult<IEnumerable<RubricaDto>>> GetAllAsync(bool showDeleted = false);
    Task<OperationResult<RubricaDto?>> GetByIdAsync(Guid id);
    Task<OperationResult<RubricaDto>> CreateAsync(RubricaCreateUpdateDto dto);
    Task<OperationResult> UpdateAsync(Guid id, RubricaCreateUpdateDto dto);
    Task<OperationResult> DeleteAsync(Guid id);
    Task<OperationResult> ToggleStatusAsync(Guid id);
}
