using FluentValidation;
using FluentValidation.Results;
using DPManagement.Application.DTOs;
using DPManagement.Application.Services;
using DPManagement.Domain.Entities;
using DPManagement.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DPManagement.API.Controllers;

[ApiController]
[Route("api/autenticacao")]
public class AuthController : ControllerBase
{
    private readonly DPManagementDbContext _context;
    private readonly IAuthService _authService;
    private readonly IValidator<LoginDto> _loginValidator;
    private readonly IValidator<RegistroUsuarioDto> _registroValidator;

    public AuthController(
        DPManagementDbContext context, 
        IAuthService authService,
        IValidator<LoginDto> loginValidator,
        IValidator<RegistroUsuarioDto> registroValidator)
    {
        _context = context;
        _authService = authService;
        _loginValidator = loginValidator;
        _registroValidator = registroValidator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        ValidationResult validationResult = await _loginValidator.ValidateAsync(loginDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => new { Campo = e.PropertyName, Erro = e.ErrorMessage }));
        }

        var usuario = await _context.Usuarios.SingleOrDefaultAsync(u => u.Email == loginDto.Email);
        
        if (usuario == null || !_authService.VerificarSenha(loginDto.Senha, usuario.SenhaHash))
        {
            return Unauthorized(new { Erro = "E-mail ou senha inválidos." });
        }

        var token = _authService.GerarTokenJwt(usuario);
        
        return Ok(new AuthRespostaDto
        {
            Token = token,
            Usuario = new UsuarioDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfil = usuario.Perfil
            }
        });
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Registro([FromBody] RegistroUsuarioDto registroDto)
    {
        ValidationResult validationResult = await _registroValidator.ValidateAsync(registroDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => new { Campo = e.PropertyName, Erro = e.ErrorMessage }));
        }

        if (await _context.Usuarios.AnyAsync(u => u.Email == registroDto.Email))
        {
            return BadRequest(new { Erro = "Um usuário com este e-mail já existe." });
        }

        var usuario = new Usuario
        {
            Nome = registroDto.Nome,
            Email = registroDto.Email,
            SenhaHash = _authService.HashSenha(registroDto.Senha),
            Perfil = registroDto.Perfil
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return Ok(new { Mensagem = "Usuário registrado com sucesso." });
    }
}
