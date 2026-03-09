namespace Tienda_DS_MS.Server.DTOs;

public record MovimientoResponseDto(int Id, string Tipo, int ReferenciaId, string Descripcion, decimal Monto, DateTime Fecha);
public record ResumenDiarioResponseDto(DateOnly Fecha, decimal TotalCompras, decimal TotalVentas, decimal Ganancia);
