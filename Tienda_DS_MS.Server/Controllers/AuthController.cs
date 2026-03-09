using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tienda_DS_MS.Server.DTOs;
using Tienda_DS_MS.Server.Services;

namespace Tienda_DS_MS.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService service, ILogger<AuthController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        try
        {
            var result = await _service.LoginAsync(dto);
            return result is null ? Unauthorized("Credenciales inválidas") : Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en login para {Email}", dto.Email);
            return StatusCode(500, new { error = ex.Message, inner = ex.InnerException?.Message });
        }
    }

    /// <summary>
    /// Endpoint de diagnóstico — verifica que la API responde y la BD está accesible.
    /// </summary>
    [HttpGet("ping")]
    public async Task<IActionResult> Ping()
    {
        try
        {
            var count = await _service.ObtenerTodosAsync();
            return Ok(new { status = "ok", usuarios = count.Count });
        }
        catch (Exception ex)
        {
            return Ok(new { status = "db_error", error = ex.Message, inner = ex.InnerException?.Message });
        }
    }

    [Authorize(Roles = "administrador")]
    [HttpPost("registro")]
    public async Task<IActionResult> Registro(RegistroDto dto)
    {
        var result = await _service.RegistrarAsync(dto);
        return CreatedAtAction(null, result);
    }

    [Authorize(Roles = "administrador")]
    [HttpGet("usuarios")]
    public async Task<IActionResult> ObtenerTodos()
        => Ok(await _service.ObtenerTodosAsync());
}
