using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tienda_DS_MS.Server.Services;

namespace Tienda_DS_MS.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "administrador")]
public class FacturasController : ControllerBase
{
    private readonly IFacturaService _service;
    public FacturasController(IFacturaService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> ObtenerTodas()
        => Ok(await _service.ObtenerTodasAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var result = await _service.ObtenerPorIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("cliente/{clienteId}")]
    public async Task<IActionResult> ObtenerPorCliente(int clienteId)
        => Ok(await _service.ObtenerPorClienteAsync(clienteId));
}
