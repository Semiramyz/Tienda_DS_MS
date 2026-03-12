using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tienda_DS_MS.Server.Data;
using Tienda_DS_MS.Server.DTOs;
using Tienda_DS_MS.Server.Models.Auth;

namespace Tienda_DS_MS.Server.Services;

public interface IAuthService
{
    Task<UsuarioResponseDto> RegistrarAsync(RegistroDto dto);
    Task<TokenResponseDto?> LoginAsync(LoginDto dto);
    Task<List<UsuarioResponseDto>> ObtenerTodosAsync();
}

public class AuthService : IAuthService
{
    private readonly AuthDbContext _db;
    private readonly IConfiguration _config;

    public AuthService(AuthDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task<UsuarioResponseDto> RegistrarAsync(RegistroDto dto)
    {
        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Rol = dto.Rol
        };

        _db.Usuarios.Add(usuario);
        await _db.SaveChangesAsync();
        return new UsuarioResponseDto(usuario.Id, usuario.Nombre, usuario.Email, usuario.Rol, usuario.Activo);
    }

    public async Task<TokenResponseDto?> LoginAsync(LoginDto dto)
    {
        var user = await _db.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email && u.Activo);
        if (user is null)
            return null;

        var valid = false;
        try
        {
            valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        }
        catch (BCrypt.Net.SaltParseException)
        {
            return null;
        }

        if (!valid)
            return null;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var expira = DateTime.UtcNow.AddHours(10);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims:
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Nombre),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Rol)
            ],
            expires: expira,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return new TokenResponseDto(new JwtSecurityTokenHandler().WriteToken(token), user.Rol, expira);
    }

    public async Task<List<UsuarioResponseDto>> ObtenerTodosAsync()
    {
        return await _db.Usuarios
            .Select(u => new UsuarioResponseDto(u.Id, u.Nombre, u.Email, u.Rol, u.Activo))
            .ToListAsync();
    }
}
