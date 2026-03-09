namespace Tienda_DS_MS.Server.Models.Clientes;

public class Cliente
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Nit { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
}
