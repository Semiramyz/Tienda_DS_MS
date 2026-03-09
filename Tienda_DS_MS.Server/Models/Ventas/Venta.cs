namespace Tienda_DS_MS.Server.Models.Ventas;

public class Venta
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int UsuarioId { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public decimal Subtotal { get; set; }
    public decimal Impuesto { get; set; }
    public decimal Total { get; set; }
    public string Estado { get; set; } = "completada";
    public List<VentaDetalle> Detalles { get; set; } = [];
}
