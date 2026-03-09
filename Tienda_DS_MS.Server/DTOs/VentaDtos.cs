namespace Tienda_DS_MS.Server.DTOs;

/// <summary>
/// Admin crea venta: selecciona cliente + lista de productos con cantidad.
/// </summary>
public record CrearVentaDto(int ClienteId, List<ItemVentaDto> Items);
public record ItemVentaDto(int ProductoId, int Cantidad);

public record VentaResponseDto(int Id, int ClienteId, string NombreCliente, DateTime Fecha, decimal Subtotal, decimal Impuesto, decimal Total, string Estado);

public record VentaDetalleResponseDto(int Id, int ProductoId, string NombreProducto, int Cantidad, decimal PrecioUnitario, decimal Subtotal);

public record VentaCompletaResponseDto(VentaResponseDto Venta, List<VentaDetalleResponseDto> Detalles, FacturaResponseDto Factura);
