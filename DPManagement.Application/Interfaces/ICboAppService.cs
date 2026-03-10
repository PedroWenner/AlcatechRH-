using DPManagement.Application.DTOs;

namespace DPManagement.Application.Interfaces;

public interface ICboAppService
{
    Task<IEnumerable<CboDto>> SearchAsync(string term);
    Task<CboDto?> GetByCodigoAsync(string codigo);
}
