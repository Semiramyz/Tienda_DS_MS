using Microsoft.EntityFrameworkCore;
using Tienda_DS_MS.Server.Data;
using Tienda_DS_MS.Server.DTOs;
using Tienda_DS_MS.Server.Models.Proveedores;

namespace Tienda_DS_MS.Server.Services;

public interface IProveedorService
{
    Task<ProveedorResponseDto> CrearAsync(CrearProveedorDto dto);
    Task<List<ProveedorResponseDto>> ObtenerTodosAsync();
    Task<ProveedorResponseDto?> ObtenerPorIdAsync(int id);
}

public class ProveedorService : IProveedorService
{
    private readonly ProveedoresDbContext _db;

    public ProveedorService(ProveedoresDbContext db) => _db = db;

    public async Task<ProveedorResponseDto> CrearAsync(CrearProveedorDto dto)
    {
        var proveedor = new Proveedor
        {
            NombreEmpresa = dto.NombreEmpresa,
            Contacto = dto.Contacto,
            Nit = dto.Nit,
            Email = dto.Email,
            Telefono = dto.Telefono,
            Direccion = dto.Direccion
        };
        _db.Proveedores.Add(proveedor);
        await _db.SaveChangesAsync();
        return ToDto(proveedor);
    }

    public async Task<List<ProveedorResponseDto>> ObtenerTodosAsync()
    {
        return await _db.Proveedores.Select(p => ToDto(p)).ToListAsync();
    }

    public async Task<ProveedorResponseDto?> ObtenerPorIdAsync(int id)
    {
        var p = await _db.Proveedores.FindAsync(id);
        return p is null ? null : ToDto(p);
    }

    private static ProveedorResponseDto ToDto(Proveedor p) =>
        new(p.Id, p.NombreEmpresa, p.Contacto, p.Nit, p.Email, p.Telefono, p.Direccion);
}
