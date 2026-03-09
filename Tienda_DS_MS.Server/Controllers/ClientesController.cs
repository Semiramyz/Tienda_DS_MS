using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tienda_DS_MS.Server.DTOs;
using Tienda_DS_MS.Server.Services;

namespace Tienda_DS_MS.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "administrador")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _service;
    public ClientesController(IClienteService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Crear(CrearClienteDto dto)
    {
        var result = await _service.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
        => Ok(await _service.ObtenerTodosAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var result = await _service.ObtenerPorIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar([FromQuery] string termino)
        => Ok(await _service.BuscarAsync(termino));
}
