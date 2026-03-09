namespace Tienda_DS_MS.Server.DTOs;

public record CrearProveedorDto(string NombreEmpresa, string? Contacto, string Nit, string? Email, string? Telefono, string? Direccion);
public record ProveedorResponseDto(int Id, string NombreEmpresa, string? Contacto, string Nit, string? Email, string? Telefono, string? Direccion);
