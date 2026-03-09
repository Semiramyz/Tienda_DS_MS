namespace Tienda_DS_MS.Server.DTOs;

/// <summary>
/// Admin compra producto a un proveedor: registra nombre, precios y cantidad.
/// </summary>
public record RegistrarCompraProductoDto(
    string Nombre,
    string? Descripcion,
    decimal PrecioCompra,
    decimal PrecioVenta,
    int Cantidad,
    int ProveedorId);

public record ProductoResponseDto(
    int Id, string Nombre, string? Descripcion,
    decimal PrecioCompra, decimal PrecioVenta,
    int Stock, int ProveedorId, bool Activo);
