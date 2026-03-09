using Microsoft.EntityFrameworkCore;
using Tienda_DS_MS.Server.Data;
using Tienda_DS_MS.Server.DTOs;
using Tienda_DS_MS.Server.Models.Contabilidad;
using Tienda_DS_MS.Server.Models.Productos;

namespace Tienda_DS_MS.Server.Services;

public interface IProductoService
{
    Task<ProductoResponseDto> RegistrarCompraAsync(RegistrarCompraProductoDto dto);
    Task<List<ProductoResponseDto>> ObtenerTodosAsync();
    Task<ProductoResponseDto?> ObtenerPorIdAsync(int id);
}

public class ProductoService : IProductoService
{
    private readonly ProductosDbContext _db;
    private readonly ContabilidadDbContext _contabilidadDb;

    public ProductoService(ProductosDbContext db, ContabilidadDbContext contabilidadDb)
    {
        _db = db;
        _contabilidadDb = contabilidadDb;
    }

    /// <summary>
    /// FLUJO DE COMPRA: Admin registra producto comprado a un proveedor.
    /// Si el producto ya existe (mismo nombre + proveedor), suma stock.
    /// Registra movimiento contable de COMPRA.
    /// </summary>
    public async Task<ProductoResponseDto> RegistrarCompraAsync(RegistrarCompraProductoDto dto)
    {
        var existente = await _db.Productos
            .FirstOrDefaultAsync(p => p.Nombre == dto.Nombre && p.ProveedorId == dto.ProveedorId);

        if (existente is not null)
        {
            existente.Stock += dto.Cantidad;
            existente.PrecioCompra = dto.PrecioCompra;
            existente.PrecioVenta = dto.PrecioVenta;
        }
        else
        {
            existente = new Producto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                PrecioCompra = dto.PrecioCompra,
                PrecioVenta = dto.PrecioVenta,
                Stock = dto.Cantidad,
                ProveedorId = dto.ProveedorId
            };
            _db.Productos.Add(existente);
        }

        await _db.SaveChangesAsync();

        // Registrar en contabilidad
        var montoTotal = dto.PrecioCompra * dto.Cantidad;
        _contabilidadDb.Movimientos.Add(new Movimiento
        {
            Tipo = "COMPRA",
            ReferenciaId = existente.Id,
            Descripcion = $"Compra de {dto.Cantidad} x {dto.Nombre} a proveedor #{dto.ProveedorId}",
            Monto = montoTotal
        });

        var hoy = DateOnly.FromDateTime(DateTime.UtcNow);
        var resumen = await _contabilidadDb.ResumenDiario.FirstOrDefaultAsync(r => r.Fecha == hoy);
        if (resumen is null)
        {
            resumen = new ResumenDiario { Fecha = hoy };
            _contabilidadDb.ResumenDiario.Add(resumen);
        }
        resumen.TotalCompras += montoTotal;
        resumen.Ganancia = resumen.TotalVentas - resumen.TotalCompras;

        await _contabilidadDb.SaveChangesAsync();

        return ToDto(existente);
    }

    public async Task<List<ProductoResponseDto>> ObtenerTodosAsync()
    {
        return await _db.Productos
            .Where(p => p.Activo)
            .Select(p => ToDto(p))
            .ToListAsync();
    }

    public async Task<ProductoResponseDto?> ObtenerPorIdAsync(int id)
    {
        var p = await _db.Productos.FindAsync(id);
        return p is null ? null : ToDto(p);
    }

    private static ProductoResponseDto ToDto(Producto p) =>
        new(p.Id, p.Nombre, p.Descripcion, p.PrecioCompra, p.PrecioVenta, p.Stock, p.ProveedorId, p.Activo);
}
