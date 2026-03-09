using DPManagement.Application.DTOs;

namespace DPManagement.Application.Interfaces;

public interface IBancoService
{
    Task<IEnumerable<BancoDto>> SearchAsync(string term);
    Task<BancoDto?> GetByCodigoAsync(string codigo);
}
