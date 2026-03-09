namespace Tienda_DS_MS.Server.DTOs;

public record FacturaResponseDto(int Id, string NumeroFactura, int VentaId, int ClienteId, string NombreCliente, string NitCliente, DateTime FechaEmision, decimal Subtotal, decimal Impuesto, decimal Total, string Estado);
