using DPManagement.Application.DTOs;

namespace DPManagement.Application.Interfaces;

public interface IColaboradorAppService
{
    Task<IEnumerable<ColaboradorDto>> GetAllAsync();
    Task<PagedResultDto<ColaboradorDto>> GetPagedAsync(int page, int pageSize, string? nome = null, string? cpf = null, Guid? cargoId = null);
    Task<ColaboradorDto?> GetByIdAsync(Guid id);
    Task AddAsync(CreateColaboradorDto colaboradorDto);
    Task UpdateAsync(UpdateColaboradorDto colaboradorDto);
    Task DeleteAsync(Guid id);
}
