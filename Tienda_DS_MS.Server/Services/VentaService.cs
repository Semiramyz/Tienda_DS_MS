using Microsoft.EntityFrameworkCore;
using Tienda_DS_MS.Server.Data;
using Tienda_DS_MS.Server.DTOs;
using Tienda_DS_MS.Server.Models.Contabilidad;
using Tienda_DS_MS.Server.Models.Facturas;
using Tienda_DS_MS.Server.Models.Ventas;

namespace Tienda_DS_MS.Server.Services;

public interface IVentaService
{
    Task<VentaCompletaResponseDto> CrearVentaAsync(CrearVentaDto dto, int usuarioId);
    Task<List<VentaResponseDto>> ObtenerTodosAsync();
    Task<VentaCompletaResponseDto?> ObtenerPorIdAsync(int id);
}

public class VentaService : IVentaService
{
    private readonly VentasDbContext _ventasDb;
    private readonly ProductosDbContext _productosDb;
    private readonly ClientesDbContext _clientesDb;
    private readonly FacturasDbContext _facturasDb;
    private readonly ContabilidadDbContext _contabilidadDb;

    public VentaService(
        VentasDbContext ventasDb,
        ProductosDbContext productosDb,
        ClientesDbContext clientesDb,
        FacturasDbContext facturasDb,
        ContabilidadDbContext contabilidadDb)
    {
        _ventasDb = ventasDb;
        _productosDb = productosDb;
        _clientesDb = clientesDb;
        _facturasDb = facturasDb;
        _contabilidadDb = contabilidadDb;
    }

    /// <summary>
    /// FLUJO COMPLETO DE VENTA:
    /// 1. Validar cliente
    /// 2. Validar stock de cada producto
    /// 3. Crear la venta con detalle
    /// 4. Descontar stock
    /// 5. Generar factura
    /// 6. Registrar en contabilidad
    /// </summary>
    public async Task<VentaCompletaResponseDto> CrearVentaAsync(CrearVentaDto dto, int usuarioId)
    {
        // 1. Validar cliente
        var cliente = await _clientesDb.Clientes.FindAsync(dto.ClienteId)
            ?? throw new Exception($"Cliente con ID {dto.ClienteId} no encontrado");

        // 2. Validar stock y construir detalles
        var detalles = new List<VentaDetalle>();
        foreach (var item in dto.Items)
        {
            var producto = await _productosDb.Productos.FindAsync(item.ProductoId)
                ?? throw new Exception($"Producto con ID {item.ProductoId} no encontrado");

            if (producto.Stock < item.Cantidad)
                throw new Exception($"Stock insuficiente para '{producto.Nombre}'. Disponible: {producto.Stock}, Solicitado: {item.Cantidad}");

            detalles.Add(new VentaDetalle
            {
                ProductoId = item.ProductoId,
                NombreProducto = producto.Nombre,
                Cantidad = item.Cantidad,
                PrecioUnitario = producto.PrecioVenta,
                Subtotal = producto.PrecioVenta * item.Cantidad
            });
        }

        // 3. Crear la venta
        var subtotal = detalles.Sum(d => d.Subtotal);
        var impuesto = subtotal * 0.12m;
        var total = subtotal + impuesto;

        var venta = new Venta
        {
            ClienteId = dto.ClienteId,
            UsuarioId = usuarioId,
            Subtotal = subtotal,
            Impuesto = impuesto,
            Total = total,
            Detalles = detalles
        };

        _ventasDb.Ventas.Add(venta);
        await _ventasDb.SaveChangesAsync();

        // 4. Descontar stock
        foreach (var item in dto.Items)
        {
            var producto = await _productosDb.Productos.FindAsync(item.ProductoId);
            producto!.Stock -= item.Cantidad;
        }
        await _productosDb.SaveChangesAsync();

        // 5. Generar factura
        var factura = new Factura
        {
            NumeroFactura = $"FAC-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}",
            VentaId = venta.Id,
            ClienteId = cliente.Id,
            NombreCliente = cliente.Nombre,
            NitCliente = cliente.Nit,
            Subtotal = subtotal,
            Impuesto = impuesto,
            Total = total
        };

        _facturasDb.Facturas.Add(factura);
        await _facturasDb.SaveChangesAsync();

        // 6. Registrar en contabilidad
        _contabilidadDb.Movimientos.Add(new Movimiento
        {
            Tipo = "VENTA",
            ReferenciaId = venta.Id,
            Descripcion = $"Venta #{venta.Id} a cliente {cliente.Nombre} (NIT: {cliente.Nit})",
            Monto = total
        });

        var hoy = DateOnly.FromDateTime(DateTime.UtcNow);
        var resumen = await _contabilidadDb.ResumenDiario.FirstOrDefaultAsync(r => r.Fecha == hoy);
        if (resumen is null)
        {
            resumen = new ResumenDiario { Fecha = hoy };
            _contabilidadDb.ResumenDiario.Add(resumen);
        }
        resumen.TotalVentas += total;
        resumen.Ganancia = resumen.TotalVentas - resumen.TotalCompras;

        await _contabilidadDb.SaveChangesAsync();

        return BuildCompleta(venta, cliente.Nombre, detalles, factura);
    }

    public async Task<List<VentaResponseDto>> ObtenerTodosAsync()
    {
        return await _ventasDb.Ventas
            .OrderByDescending(v => v.Fecha)
            .Select(v => new VentaResponseDto(v.Id, v.ClienteId, "", v.Fecha, v.Subtotal, v.Impuesto, v.Total, v.Estado))
            .ToListAsync();
    }

    public async Task<VentaCompletaResponseDto?> ObtenerPorIdAsync(int id)
    {
        var venta = await _ventasDb.Ventas.Include(v => v.Detalles).FirstOrDefaultAsync(v => v.Id == id);
        if (venta is null) return null;

        var cliente = await _clientesDb.Clientes.FindAsync(venta.ClienteId);
        var factura = await _facturasDb.Facturas.FirstOrDefaultAsync(f => f.VentaId == venta.Id);

        return BuildCompleta(venta, cliente?.Nombre ?? "", venta.Detalles, factura);
    }

    private static VentaCompletaResponseDto BuildCompleta(Venta v, string nombreCliente, List<VentaDetalle> detalles, Factura? factura)
    {
        var ventaDto = new VentaResponseDto(v.Id, v.ClienteId, nombreCliente, v.Fecha, v.Subtotal, v.Impuesto, v.Total, v.Estado);
        var detallesDto = detalles.Select(d =>
            new VentaDetalleResponseDto(d.Id, d.ProductoId, d.NombreProducto, d.Cantidad, d.PrecioUnitario, d.Subtotal)).ToList();
        var facturaDto = factura is null ? null! :
            new FacturaResponseDto(factura.Id, factura.NumeroFactura, factura.VentaId, factura.ClienteId,
                factura.NombreCliente, factura.NitCliente, factura.FechaEmision,
                factura.Subtotal, factura.Impuesto, factura.Total, factura.Estado);

        return new VentaCompletaResponseDto(ventaDto, detallesDto, facturaDto);
    }
}
