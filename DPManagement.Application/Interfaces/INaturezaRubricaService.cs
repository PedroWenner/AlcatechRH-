using System.Collections.Generic;
using System.Threading.Tasks;
using DPManagement.Application.DTOs;

namespace DPManagement.Application.Interfaces
{
    public interface INaturezaRubricaService
    {
        Task<IEnumerable<NaturezaRubricaDto>> GetAllAsync();
        Task<NaturezaRubricaDto?> GetByCodigoAsync(string codigo);
    }
}
