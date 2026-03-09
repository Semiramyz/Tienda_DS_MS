using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tienda_DS_MS.Server.DTOs;
using Tienda_DS_MS.Server.Services;

namespace Tienda_DS_MS.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "administrador")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _service;
    public ProductosController(IProductoService service) => _service = service;

    /// <summary>
    /// Admin compra a proveedor → registra producto + stock + contabilidad.
    /// </summary>
    [HttpPost("compra")]
    public async Task<IActionResult> RegistrarCompra(RegistrarCompraProductoDto dto)
    {
        var result = await _service.RegistrarCompraAsync(dto);
        return CreatedAtAction(nameof(ObtenerTodos), result);
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
