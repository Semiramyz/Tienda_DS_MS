using Microsoft.EntityFrameworkCore;
using Tienda_DS_MS.Server.Models.Proveedores;

namespace Tienda_DS_MS.Server.Data;

public class ProveedoresDbContext : DbContext
{
    public ProveedoresDbContext(DbContextOptions<ProveedoresDbContext> options) : base(options) { }

    public DbSet<Proveedor> Proveedores => Set<Proveedor>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Proveedor>(e =>
        {
            e.ToTable("proveedores");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.NombreEmpresa).HasColumnName("nombre_empresa").HasMaxLength(200).IsRequired();
            e.Property(x => x.Contacto).HasColumnName("contacto").HasMaxLength(150);
            e.Property(x => x.Nit).HasColumnName("nit").HasMaxLength(20).IsRequired();
            e.Property(x => x.Email).HasColumnName("email").HasMaxLength(150);
            e.Property(x => x.Telefono).HasColumnName("telefono").HasMaxLength(20);
            e.Property(x => x.Direccion).HasColumnName("direccion").HasMaxLength(300);
            e.Property(x => x.FechaRegistro).HasColumnName("fecha_registro");
        });
    }
}
