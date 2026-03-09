namespace Tienda_DS_MS.Server.Models.Contabilidad;

public class Movimiento
{
    public int Id { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public int ReferenciaId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
}
