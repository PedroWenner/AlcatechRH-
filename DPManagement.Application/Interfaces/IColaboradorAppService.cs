using DPManagement.Application.Common;
using DPManagement.Application.DTOs;

namespace DPManagement.Application.Interfaces;

public interface IColaboradorAppService
{
    Task<OperationResult<IEnumerable<ColaboradorDto>>> GetAllAsync();
    Task<OperationResult<PagedResultDto<ColaboradorDto>>> GetPagedAsync(int page, int pageSize, string? nome = null, string? cpf = null, Guid? cargoId = null);
    Task<OperationResult<ColaboradorDto?>> GetByIdAsync(Guid id);
    Task<OperationResult> AddAsync(CreateColaboradorDto colaboradorDto);
    Task<OperationResult> UpdateAsync(UpdateColaboradorDto colaboradorDto);
    Task<OperationResult> DeleteAsync(Guid id);
}
