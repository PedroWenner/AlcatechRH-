using DPManagement.Application.Common;
using DPManagement.Application.DTOs;

namespace DPManagement.Application.Interfaces;

public interface IBancoService
{
    Task<OperationResult<IEnumerable<BancoDto>>> SearchAsync(string term);
    Task<OperationResult<BancoDto>> GetByCodigoAsync(string codigo);
}
