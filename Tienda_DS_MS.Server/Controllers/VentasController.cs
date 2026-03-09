using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tienda_DS_MS.Server.DTOs;
using Tienda_DS_MS.Server.Services;

namespace Tienda_DS_MS.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "administrador")]
public class VentasController : ControllerBase
{
    private readonly IVentaService _service;
    public VentasController(IVentaService service) => _service = service;

    /// <summary>
    /// Admin selecciona cliente + productos → crea venta, descuenta stock,
    /// genera factura y registra en contabilidad.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CrearVenta(CrearVentaDto dto)
    {
        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        try
        {
            var result = await _service.CrearVentaAsync(dto, usuarioId);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = result.Venta.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
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
}
