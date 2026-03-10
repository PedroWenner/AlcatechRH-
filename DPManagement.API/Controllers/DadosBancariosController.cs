using DPManagement.Application.Common;
using DPManagement.Application.DTOs;
using DPManagement.Application.Interfaces;
using DPManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DPManagement.API.Controllers;

[ApiController]
[Route("api/colaboradores/{colaboradorId}/dados-bancarios")]
public class DadosBancariosController : ControllerBase
{
    private readonly IDadoBancarioService _service;

    public DadosBancariosController(IDadoBancarioService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Listar(Guid colaboradorId)
    {
        var result = await _service.ListarPorColaboradorIdAsync(colaboradorId);
        if (!result.Success) return BadRequest(result);

        var dtos = result.Data.Select(db => new DadoBancarioDto
        {
            Id = db.Id,
            ColaboradorId = db.ColaboradorId,
            BancoId = db.BancoId,
            NomeBanco = db.Banco?.Nome ?? string.Empty,
            CodigoBanco = db.CodigoBanco,
            Agencia = db.Agencia,
            DigitoAgencia = db.DigitoAgencia,
            Conta = db.Conta,
            DigitoConta = db.DigitoConta,
            Operacao = db.Operacao,
            Ativo = db.Ativo
        });

        return Ok(OperationResult<IEnumerable<DadoBancarioDto>>.Ok(dtos));
    }

    [HttpPost]
    public async Task<IActionResult> Criar(Guid colaboradorId, [FromBody] DadoBancarioRequestDto request)
    {
        var entidade = new DadoBancario
        {
            ColaboradorId = colaboradorId, 
            BancoId = request.BancoId,
            CodigoBanco = request.CodigoBanco,
            Agencia = request.Agencia,
            DigitoAgencia = request.DigitoAgencia,
            Conta = request.Conta,
            DigitoConta = request.DigitoConta,
            Operacao = request.Operacao
        };

        var result = await _service.AdicionarAsync(entidade);
        if (!result.Success) return BadRequest(result);
        return CreatedAtAction(nameof(Listar), new { colaboradorId = colaboradorId }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(Guid colaboradorId, Guid id, [FromBody] DadoBancarioRequestDto request)
    {
        var getResult = await _service.ObterPorIdAsync(id);
        if (!getResult.Success || getResult.Data?.ColaboradorId != colaboradorId) 
            return NotFound(OperationResult.Failure("Dado bancário não encontrado."));

        var dado = getResult.Data!;
        dado.BancoId = request.BancoId;
        dado.CodigoBanco = request.CodigoBanco;
        dado.Agencia = request.Agencia;
        dado.DigitoAgencia = request.DigitoAgencia;
        dado.Conta = request.Conta;
        dado.DigitoConta = request.DigitoConta;
        dado.Operacao = request.Operacao;

        var result = await _service.AtualizarAsync(dado);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(Guid colaboradorId, Guid id)
    {
        var result = await _service.RemoverAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> AlternarStatus(Guid colaboradorId, Guid id, [FromBody] bool ativo)
    {
        var result = await _service.AlternarStatusAsync(id, ativo);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
