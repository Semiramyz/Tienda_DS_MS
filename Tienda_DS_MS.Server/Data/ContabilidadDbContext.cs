using Microsoft.EntityFrameworkCore;
using Tienda_DS_MS.Server.Models.Contabilidad;

namespace Tienda_DS_MS.Server.Data;

public class ContabilidadDbContext : DbContext
{
    public ContabilidadDbContext(DbContextOptions<ContabilidadDbContext> options) : base(options) { }

    public DbSet<Movimiento> Movimientos => Set<Movimiento>();
    public DbSet<ResumenDiario> ResumenDiario => Set<ResumenDiario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movimiento>(e =>
        {
            e.ToTable("movimientos");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Tipo).HasColumnName("tipo").HasMaxLength(10).IsRequired();
            e.Property(x => x.ReferenciaId).HasColumnName("referencia_id");
            e.Property(x => x.Descripcion).HasColumnName("descripcion").HasMaxLength(300);
            e.Property(x => x.Monto).HasColumnName("monto").HasColumnType("decimal(12,2)");
            e.Property(x => x.Fecha).HasColumnName("fecha");
        });

        modelBuilder.Entity<ResumenDiario>(e =>
        {
            e.ToTable("resumen_diario");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Fecha).HasColumnName("fecha");
            e.HasIndex(x => x.Fecha).IsUnique();
            e.Property(x => x.TotalCompras).HasColumnName("total_compras").HasColumnType("decimal(12,2)");
            e.Property(x => x.TotalVentas).HasColumnName("total_ventas").HasColumnType("decimal(12,2)");
            e.Property(x => x.Ganancia).HasColumnName("ganancia").HasColumnType("decimal(12,2)");
        });
    }
}
