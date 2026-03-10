using DPManagement.Application.DTOs;

namespace DPManagement.Application.Interfaces;

public interface IVinculoService
{
    Task<PagedResultDto<VinculoDto>> GetPaginatedAsync(int page, int pageSize, string? matricula, string? nomeColaborador);
    Task<IEnumerable<VinculoDto>> GetAllAsync(bool showDeleted = false);
    Task<VinculoDto?> GetByIdAsync(Guid id);
    Task<VinculoDto> CreateAsync(VinculoCreateUpdateDto dto);
    Task UpdateAsync(Guid id, VinculoCreateUpdateDto dto);
    Task ToggleStatusAsync(Guid id);
    Task DeleteAsync(Guid id);
}
