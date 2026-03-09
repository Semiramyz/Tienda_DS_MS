namespace Tienda_DS_MS.Server.Models.Proveedores;

public class Proveedor
{
    public int Id { get; set; }
    public string NombreEmpresa { get; set; } = string.Empty;
    public string? Contacto { get; set; }
    public string Nit { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
}
