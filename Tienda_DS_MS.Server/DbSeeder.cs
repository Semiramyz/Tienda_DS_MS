using Microsoft.EntityFrameworkCore;
using Tienda_DS_MS.Server.Data;
using Tienda_DS_MS.Server.Models.Auth;

namespace Tienda_DS_MS.Server;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

        // Crear el admin por defecto si no existe ningún usuario
        if (!await db.Usuarios.AnyAsync())
        {
            db.Usuarios.Add(new Usuario
            {
                Nombre = "Administrador",
                Email = "admin@tienda.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Rol = "administrador"
            });
            await db.SaveChangesAsync();
        }
    }
}
