using Microsoft.EntityFrameworkCore;
using Tienda_DS_MS.Server.Models.Productos;

namespace Tienda_DS_MS.Server.Data;

public class ProductosDbContext : DbContext
{
    public ProductosDbContext(DbContextOptions<ProductosDbContext> options) : base(options) { }

    public DbSet<Producto> Productos => Set<Producto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>(e =>
        {
            e.ToTable("productos");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(200).IsRequired();
            e.Property(x => x.Descripcion).HasColumnName("descripcion");
            e.Property(x => x.PrecioCompra).HasColumnName("precio_compra").HasColumnType("decimal(10,2)");
            e.Property(x => x.PrecioVenta).HasColumnName("precio_venta").HasColumnType("decimal(10,2)");
            e.Property(x => x.Stock).HasColumnName("stock");
            e.Property(x => x.ProveedorId).HasColumnName("proveedor_id");
            e.Property(x => x.Activo).HasColumnName("activo");
            e.Property(x => x.FechaRegistro).HasColumnName("fecha_registro");
        });
    }
}
