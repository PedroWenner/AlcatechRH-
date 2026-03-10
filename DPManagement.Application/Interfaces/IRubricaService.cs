using DPManagement.Application.DTOs;

namespace DPManagement.Application.Interfaces;

public interface IRubricaService
{
    Task<PagedResultDto<RubricaDto>> GetPaginatedAsync(int page, int pageSize, string? filtro = null);
    Task<IEnumerable<RubricaDto>> GetAllAsync(bool showDeleted = false);
    Task<RubricaDto?> GetByIdAsync(Guid id);
    Task<RubricaDto> CreateAsync(RubricaCreateUpdateDto dto);
    Task UpdateAsync(Guid id, RubricaCreateUpdateDto dto);
    Task DeleteAsync(Guid id);
    Task ToggleStatusAsync(Guid id);
}
