using Microsoft.EntityFrameworkCore;
using Tienda_DS_MS.Server.Data;
using Tienda_DS_MS.Server.DTOs;
using Tienda_DS_MS.Server.Models.Clientes;

namespace Tienda_DS_MS.Server.Services;

public interface IClienteService
{
    Task<ClienteResponseDto> CrearAsync(CrearClienteDto dto);
    Task<List<ClienteResponseDto>> ObtenerTodosAsync();
    Task<ClienteResponseDto?> ObtenerPorIdAsync(int id);
    Task<List<ClienteResponseDto>> BuscarAsync(string termino);
}

public class ClienteService : IClienteService
{
    private readonly ClientesDbContext _db;

    public ClienteService(ClientesDbContext db) => _db = db;

    public async Task<ClienteResponseDto> CrearAsync(CrearClienteDto dto)
    {
        var cliente = new Cliente
        {
            Nombre = dto.Nombre,
            Nit = dto.Nit,
            Email = dto.Email,
            Telefono = dto.Telefono,
            Direccion = dto.Direccion
        };
        _db.Clientes.Add(cliente);
        await _db.SaveChangesAsync();
        return ToDto(cliente);
    }

    public async Task<List<ClienteResponseDto>> ObtenerTodosAsync()
    {
        return await _db.Clientes.Select(c => ToDto(c)).ToListAsync();
    }

    public async Task<ClienteResponseDto?> ObtenerPorIdAsync(int id)
    {
        var c = await _db.Clientes.FindAsync(id);
        return c is null ? null : ToDto(c);
    }

    public async Task<List<ClienteResponseDto>> BuscarAsync(string termino)
    {
        return await _db.Clientes
            .Where(c => c.Nombre.Contains(termino) || c.Nit.Contains(termino))
            .Select(c => ToDto(c))
            .ToListAsync();
    }

    private static ClienteResponseDto ToDto(Cliente c) =>
        new(c.Id, c.Nombre, c.Nit, c.Email, c.Telefono, c.Direccion);
}
