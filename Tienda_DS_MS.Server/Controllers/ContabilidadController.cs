using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tienda_DS_MS.Server.Services;

namespace Tienda_DS_MS.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "administrador")]
public class ContabilidadController : ControllerBase
{
    private readonly IContabilidadService _service;
    public ContabilidadController(IContabilidadService service) => _service = service;

    [HttpGet("movimientos")]
    public async Task<IActionResult> ObtenerMovimientos()
        => Ok(await _service.ObtenerMovimientosAsync());

    [HttpGet("resumen")]
    public async Task<IActionResult> ObtenerResumen([FromQuery] DateOnly? desde, [FromQuery] DateOnly? hasta)
        => Ok(await _service.ObtenerResumenAsync(desde, hasta));
}
