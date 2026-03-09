using Microsoft.EntityFrameworkCore;
using Tienda_DS_MS.Server.Models.Ventas;

namespace Tienda_DS_MS.Server.Data;

public class VentasDbContext : DbContext
{
    public VentasDbContext(DbContextOptions<VentasDbContext> options) : base(options) { }

    public DbSet<Venta> Ventas => Set<Venta>();
    public DbSet<VentaDetalle> VentaDetalles => Set<VentaDetalle>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Venta>(e =>
        {
            e.ToTable("ventas");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.ClienteId).HasColumnName("cliente_id");
            e.Property(x => x.UsuarioId).HasColumnName("usuario_id");
            e.Property(x => x.Fecha).HasColumnName("fecha");
            e.Property(x => x.Subtotal).HasColumnName("subtotal").HasColumnType("decimal(12,2)");
            e.Property(x => x.Impuesto).HasColumnName("impuesto").HasColumnType("decimal(12,2)");
            e.Property(x => x.Total).HasColumnName("total").HasColumnType("decimal(12,2)");
            e.Property(x => x.Estado).HasColumnName("estado").HasMaxLength(20);
            e.HasMany(x => x.Detalles).WithOne(x => x.Venta).HasForeignKey(x => x.VentaId);
        });

        modelBuilder.Entity<VentaDetalle>(e =>
        {
            e.ToTable("venta_detalle");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.VentaId).HasColumnName("venta_id");
            e.Property(x => x.ProductoId).HasColumnName("producto_id");
            e.Property(x => x.NombreProducto).HasColumnName("nombre_producto").HasMaxLength(200);
            e.Property(x => x.Cantidad).HasColumnName("cantidad");
            e.Property(x => x.PrecioUnitario).HasColumnName("precio_unitario").HasColumnType("decimal(10,2)");
            e.Property(x => x.Subtotal).HasColumnName("subtotal").HasColumnType("decimal(12,2)");
        });
    }
}
