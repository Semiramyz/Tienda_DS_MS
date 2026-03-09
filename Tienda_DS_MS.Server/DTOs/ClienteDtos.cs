namespace Tienda_DS_MS.Server.DTOs;

public record CrearClienteDto(string Nombre, string Nit, string? Email, string? Telefono, string? Direccion);
public record ClienteResponseDto(int Id, string Nombre, string Nit, string? Email, string? Telefono, string? Direccion);
