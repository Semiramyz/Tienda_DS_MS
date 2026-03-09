using Microsoft.EntityFrameworkCore;
using Tienda_DS_MS.Server.Models.Auth;

namespace Tienda_DS_MS.Server.Data;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(e =>
        {
            e.ToTable("usuarios");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
            e.Property(x => x.Email).HasColumnName("email").HasMaxLength(150).IsRequired();
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.PasswordHash).HasColumnName("password_hash").HasMaxLength(256).IsRequired();
            e.Property(x => x.Rol).HasColumnName("rol").HasMaxLength(20);
            e.Property(x => x.Activo).HasColumnName("activo");
            e.Property(x => x.FechaCreacion).HasColumnName("fecha_creacion");
        });
    }
}
