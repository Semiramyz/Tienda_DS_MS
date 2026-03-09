using Microsoft.EntityFrameworkCore;
using Tienda_DS_MS.Server.Models.Clientes;

namespace Tienda_DS_MS.Server.Data;

public class ClientesDbContext : DbContext
{
    public ClientesDbContext(DbContextOptions<ClientesDbContext> options) : base(options) { }

    public DbSet<Cliente> Clientes => Set<Cliente>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(e =>
        {
            e.ToTable("clientes");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(150).IsRequired();
            e.Property(x => x.Nit).HasColumnName("nit").HasMaxLength(20).IsRequired();
            e.Property(x => x.Email).HasColumnName("email").HasMaxLength(150);
            e.Property(x => x.Telefono).HasColumnName("telefono").HasMaxLength(20);
            e.Property(x => x.Direccion).HasColumnName("direccion").HasMaxLength(300);
            e.Property(x => x.FechaRegistro).HasColumnName("fecha_registro");
        });
    }
}
