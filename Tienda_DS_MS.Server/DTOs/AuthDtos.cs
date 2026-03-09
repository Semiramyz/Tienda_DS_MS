namespace Tienda_DS_MS.Server.DTOs;

public record LoginDto(string Email, string Password);
public record RegistroDto(string Nombre, string Email, string Password, string Rol);
public record TokenResponseDto(string Token, string Rol, DateTime Expira);
public record UsuarioResponseDto(int Id, string Nombre, string Email, string Rol, bool Activo);
