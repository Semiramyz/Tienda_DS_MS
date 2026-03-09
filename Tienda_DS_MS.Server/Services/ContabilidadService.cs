using Microsoft.EntityFrameworkCore;
using Tienda_DS_MS.Server.Data;
using Tienda_DS_MS.Server.DTOs;

namespace Tienda_DS_MS.Server.Services;

public interface IContabilidadService
{
    Task<List<MovimientoResponseDto>> ObtenerMovimientosAsync();
    Task<List<ResumenDiarioResponseDto>> ObtenerResumenAsync(DateOnly? desde, DateOnly? hasta);
}

public class ContabilidadService : IContabilidadService
{
    private readonly ContabilidadDbContext _db;

    public ContabilidadService(ContabilidadDbContext db) => _db = db;

    public async Task<List<MovimientoResponseDto>> ObtenerMovimientosAsync()
    {
        return await _db.Movimientos
            .OrderByDescending(m => m.Fecha)
            .Select(m => new MovimientoResponseDto(m.Id, m.Tipo, m.ReferenciaId, m.Descripcion, m.Monto, m.Fecha))
            .ToListAsync();
    }

    public async Task<List<ResumenDiarioResponseDto>> ObtenerResumenAsync(DateOnly? desde, DateOnly? hasta)
    {
        var query = _db.ResumenDiario.AsQueryable();
        if (desde.HasValue) query = query.Where(r => r.Fecha >= desde.Value);
        if (hasta.HasValue) query = query.Where(r => r.Fecha <= hasta.Value);
        return await query
            .OrderByDescending(r => r.Fecha)
            .Select(r => new ResumenDiarioResponseDto(r.Fecha, r.TotalCompras, r.TotalVentas, r.Ganancia))
            .ToListAsync();
    }
}
