using DPManagement.Application.Common;
using DPManagement.Application.DTOs;

namespace DPManagement.Application.Interfaces;

public interface ICargoAppService
{
    Task<OperationResult<IEnumerable<CargoDto>>> GetAllAsync();
    Task<OperationResult<PagedResultDto<CargoDto>>> GetPagedAsync(int page, int pageSize, string? nome = null, string? cbo = null);
    Task<OperationResult<CargoDto?>> GetByIdAsync(Guid id);
    Task<OperationResult> AddAsync(CreateCargoDto cargoDto);
    Task<OperationResult> UpdateAsync(UpdateCargoDto cargoDto);
    Task<OperationResult> DeleteAsync(Guid id);
}
