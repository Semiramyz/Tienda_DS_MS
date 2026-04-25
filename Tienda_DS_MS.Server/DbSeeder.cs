using Microsoft.EntityFrameworkCore;
using Tienda_DS_MS.Server.Data;
using Tienda_DS_MS.Server.Models.Auth;
using Tienda_DS_MS.Server.Models.Contabilidad;

namespace Tienda_DS_MS.Server;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        // Seed Auth DB
        await SeedAuthDbAsync(scope);

        // Seed Contabilidad DB
        await SeedContabilidadDbAsync(scope);
    }

    private static async Task SeedAuthDbAsync(IServiceScope scope)
    {
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

    private static async Task SeedContabilidadDbAsync(IServiceScope scope)
    {
        var db = scope.ServiceProvider.GetRequiredService<ContabilidadDbContext>();

        try
        {
            // Seed movimientos si no existen
            if (!await db.Movimientos.AnyAsync())
            {
                var hoy = DateTime.UtcNow;
                var movimientos = new List<Movimiento>
                {
                    new Movimiento
                    {
                        Tipo = "COMPRA",
                        ReferenciaId = 1,
                        Descripcion = "Compra de inventario inicial",
                        Monto = 5000m,
                        Fecha = hoy.AddDays(-5)
                    },
                    new Movimiento
                    {
                        Tipo = "VENTA",
                        ReferenciaId = 1,
                        Descripcion = "Venta de prueba 1",
                        Monto = 2500m,
                        Fecha = hoy.AddDays(-4)
                    },
                    new Movimiento
                    {
                        Tipo = "VENTA",
                        ReferenciaId = 2,
                        Descripcion = "Venta de prueba 2",
                        Monto = 1800m,
                        Fecha = hoy.AddDays(-3)
                    },
                    new Movimiento
                    {
                        Tipo = "COMPRA",
                        ReferenciaId = 2,
                        Descripcion = "Compra adicional",
                        Monto = 3200m,
                        Fecha = hoy.AddDays(-2)
                    },
                    new Movimiento
                    {
                        Tipo = "VENTA",
                        ReferenciaId = 3,
                        Descripcion = "Venta de prueba 3",
                        Monto = 3500m,
                        Fecha = hoy.AddDays(-1)
                    },
                    new Movimiento
                    {
                        Tipo = "VENTA",
                        ReferenciaId = 4,
                        Descripcion = "Venta de hoy",
                        Monto = 2200m,
                        Fecha = hoy
                    }
                };

                db.Movimientos.AddRange(movimientos);
                await db.SaveChangesAsync();
            }

            // Seed resumen diario si no existen
            if (!await db.ResumenDiario.AnyAsync())
            {
                var hoy = DateOnly.FromDateTime(DateTime.UtcNow);
                var resumenes = new List<ResumenDiario>
                {
                    new ResumenDiario
                    {
                        Fecha = hoy.AddDays(-5),
                        TotalCompras = 5000m,
                        TotalVentas = 0m,
                        Ganancia = -5000m
                    },
                    new ResumenDiario
                    {
                        Fecha = hoy.AddDays(-4),
                        TotalCompras = 0m,
                        TotalVentas = 2500m,
                        Ganancia = 2500m
                    },
                    new ResumenDiario
                    {
                        Fecha = hoy.AddDays(-3),
                        TotalCompras = 0m,
                        TotalVentas = 1800m,
                        Ganancia = 1800m
                    },
                    new ResumenDiario
                    {
                        Fecha = hoy.AddDays(-2),
                        TotalCompras = 3200m,
                        TotalVentas = 0m,
                        Ganancia = -3200m
                    },
                    new ResumenDiario
                    {
                        Fecha = hoy.AddDays(-1),
                        TotalCompras = 0m,
                        TotalVentas = 3500m,
                        Ganancia = 3500m
                    },
                    new ResumenDiario
                    {
                        Fecha = hoy,
                        TotalCompras = 0m,
                        TotalVentas = 2200m,
                        Ganancia = 2200m
                    }
                };

                db.ResumenDiario.AddRange(resumenes);
                await db.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            // Log error pero no fallar la aplicación
            System.Console.WriteLine($"Error seeding Contabilidad data: {ex.Message}");
        }
    }
}
