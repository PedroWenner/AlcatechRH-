using DPManagement.Application.Common;
using DPManagement.Application.DTOs;

namespace DPManagement.Application.Interfaces;

public interface ICboAppService
{
    Task<OperationResult<IEnumerable<CboDto>>> SearchAsync(string term);
    Task<OperationResult<CboDto>> GetByCodigoAsync(string codigo);
}
