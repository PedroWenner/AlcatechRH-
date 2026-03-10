using DPManagement.Application.Common;
using DPManagement.Application.DTOs;
using DPManagement.Application.Interfaces;
using DPManagement.Application.Common;
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

    public async Task<OperationResult<IEnumerable<CboDto>>> SearchAsync(string term)
    {
        var cbos = await _cboRepository.SearchAsync(term);
        var dtos = cbos.Select(c => new CboDto
        {
            Id = c.Id,
            Codigo = c.Codigo,
            Titulo = c.Titulo
        });
        return OperationResult<IEnumerable<CboDto>>.Ok(dtos);
    }

    public async Task<OperationResult<CboDto>> GetByCodigoAsync(string codigo)
    {
        var c = await _cboRepository.GetByCodigoAsync(codigo);
        if (c == null) return OperationResult<CboDto>.Failure("CBO não encontrado.");

        var dto = new CboDto
        {
            Id = c.Id,
            Codigo = c.Codigo,
            Titulo = c.Titulo
        };
        return OperationResult<CboDto>.Ok(dto);
    }
}
