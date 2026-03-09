using Microsoft.EntityFrameworkCore;
using Tienda_DS_MS.Server.Models.Facturas;

namespace Tienda_DS_MS.Server.Data;

public class FacturasDbContext : DbContext
{
    public FacturasDbContext(DbContextOptions<FacturasDbContext> options) : base(options) { }

    public DbSet<Factura> Facturas => Set<Factura>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Factura>(e =>
        {
            e.ToTable("facturas");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.NumeroFactura).HasColumnName("numero_factura").HasMaxLength(50).IsRequired();
            e.HasIndex(x => x.NumeroFactura).IsUnique();
            e.Property(x => x.VentaId).HasColumnName("venta_id");
            e.Property(x => x.ClienteId).HasColumnName("cliente_id");
            e.Property(x => x.NombreCliente).HasColumnName("nombre_cliente").HasMaxLength(150);
            e.Property(x => x.NitCliente).HasColumnName("nit_cliente").HasMaxLength(20);
            e.Property(x => x.FechaEmision).HasColumnName("fecha_emision");
            e.Property(x => x.Subtotal).HasColumnName("subtotal").HasColumnType("decimal(12,2)");
            e.Property(x => x.Impuesto).HasColumnName("impuesto").HasColumnType("decimal(12,2)");
            e.Property(x => x.Total).HasColumnName("total").HasColumnType("decimal(12,2)");
            e.Property(x => x.Estado).HasColumnName("estado").HasMaxLength(20);
        });
    }
}
