using DPManagement.Application.DTOs;

namespace DPManagement.Application.Interfaces;

public interface IColaboradorAppService
{
    Task<IEnumerable<ColaboradorDto>> GetAllAsync();
    Task<PagedResultDto<ColaboradorDto>> GetPagedAsync(int page, int pageSize);
    Task<ColaboradorDto?> GetByIdAsync(Guid id);
    Task AddAsync(CreateColaboradorDto colaboradorDto);
    Task UpdateAsync(UpdateColaboradorDto colaboradorDto);
    Task DeleteAsync(Guid id);
}
