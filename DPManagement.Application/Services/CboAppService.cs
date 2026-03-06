using DPManagement.Application.DTOs;
using DPManagement.Application.Interfaces;
using DPManagement.Domain.Repositories;

namespace DPManagement.Application.Services;

public class CboAppService : ICboAppService
{
    private readonly ICboRepository _cboRepository;

    public CboAppService(ICboRepository cboRepository)
    {
        _cboRepository = cboRepository;
    }

    public async Task<IEnumerable<CboDto>> SearchAsync(string term)
    {
        var cbos = await _cboRepository.SearchAsync(term);
        return cbos.Select(c => new CboDto
        {
            Id = c.Id,
            Codigo = c.Codigo,
            Titulo = c.Titulo
        });
    }

    public async Task<CboDto?> GetByCodigoAsync(string codigo)
    {
        var c = await _cboRepository.GetByCodigoAsync(codigo);
        if (c == null) return null;

        return new CboDto
        {
            Id = c.Id,
            Codigo = c.Codigo,
            Titulo = c.Titulo
        };
    }
}
