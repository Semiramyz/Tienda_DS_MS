using Microsoft.EntityFrameworkCore;
using Tienda_DS_MS.Server.Data;
using Tienda_DS_MS.Server.DTOs;

namespace Tienda_DS_MS.Server.Services;

public interface IFacturaService
{
    Task<List<FacturaResponseDto>> ObtenerTodasAsync();
    Task<FacturaResponseDto?> ObtenerPorIdAsync(int id);
    Task<List<FacturaResponseDto>> ObtenerPorClienteAsync(int clienteId);
}

public class FacturaService : IFacturaService
{
    private readonly FacturasDbContext _db;

    public FacturaService(FacturasDbContext db) => _db = db;

    public async Task<List<FacturaResponseDto>> ObtenerTodasAsync()
    {
        return await _db.Facturas
            .OrderByDescending(f => f.FechaEmision)
            .Select(f => new FacturaResponseDto(
                f.Id, f.NumeroFactura, f.VentaId, f.ClienteId,
                f.NombreCliente, f.NitCliente, f.FechaEmision,
                f.Subtotal, f.Impuesto, f.Total, f.Estado))
            .ToListAsync();
    }

    public async Task<FacturaResponseDto?> ObtenerPorIdAsync(int id)
    {
        var f = await _db.Facturas.FindAsync(id);
        return f is null ? null : new FacturaResponseDto(
            f.Id, f.NumeroFactura, f.VentaId, f.ClienteId,
            f.NombreCliente, f.NitCliente, f.FechaEmision,
            f.Subtotal, f.Impuesto, f.Total, f.Estado);
    }

    public async Task<List<FacturaResponseDto>> ObtenerPorClienteAsync(int clienteId)
    {
        return await _db.Facturas
            .Where(f => f.ClienteId == clienteId)
            .OrderByDescending(f => f.FechaEmision)
            .Select(f => new FacturaResponseDto(
                f.Id, f.NumeroFactura, f.VentaId, f.ClienteId,
                f.NombreCliente, f.NitCliente, f.FechaEmision,
                f.Subtotal, f.Impuesto, f.Total, f.Estado))
            .ToListAsync();
    }
}
