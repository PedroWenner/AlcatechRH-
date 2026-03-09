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
        var dados = await _service.ListarPorColaboradorIdAsync(colaboradorId);
        var dtos = dados.Select(db => new DadoBancarioDto
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

        return Ok(dtos);
    }

    [HttpPost]
    public async Task<IActionResult> Criar(Guid colaboradorId, [FromBody] DadoBancarioRequestDto request)
    {
        var entidade = new DadoBancario
        {
            ColaboradorId = colaboradorId, // Enforce route param
            BancoId = request.BancoId,
            CodigoBanco = request.CodigoBanco,
            Agencia = request.Agencia,
            DigitoAgencia = request.DigitoAgencia,
            Conta = request.Conta,
            DigitoConta = request.DigitoConta,
            Operacao = request.Operacao
        };

        await _service.AdicionarAsync(entidade);
        return CreatedAtAction(nameof(Listar), new { colaboradorId = colaboradorId }, entidade);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(Guid colaboradorId, Guid id, [FromBody] DadoBancarioRequestDto request)
    {
        var dado = await _service.ObterPorIdAsync(id);
        if (dado == null || dado.ColaboradorId != colaboradorId) 
            return NotFound();

        dado.BancoId = request.BancoId;
        dado.CodigoBanco = request.CodigoBanco;
        dado.Agencia = request.Agencia;
        dado.DigitoAgencia = request.DigitoAgencia;
        dado.Conta = request.Conta;
        dado.DigitoConta = request.DigitoConta;
        dado.Operacao = request.Operacao;

        await _service.AtualizarAsync(dado);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(Guid colaboradorId, Guid id)
    {
        var dado = await _service.ObterPorIdAsync(id);
        if (dado == null || dado.ColaboradorId != colaboradorId)
            return NotFound();

        await _service.RemoverAsync(id);
        return NoContent();
    }

    [HttpPut("{id}/ativar")]
    public async Task<IActionResult> AtivarInativar(Guid colaboradorId, Guid id, [FromQuery] bool ativo)
    {
        var dado = await _service.ObterPorIdAsync(id);
        if (dado == null || dado.ColaboradorId != colaboradorId)
            return NotFound();

        await _service.AlternarStatusAsync(id, ativo);
        return NoContent();
    }
}
