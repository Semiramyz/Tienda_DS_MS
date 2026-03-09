namespace Tienda_DS_MS.Server.Models.Facturas;

public class Factura
{
    public int Id { get; set; }
    public string NumeroFactura { get; set; } = string.Empty;
    public int VentaId { get; set; }
    public int ClienteId { get; set; }
    public string NombreCliente { get; set; } = string.Empty;
    public string NitCliente { get; set; } = string.Empty;
    public DateTime FechaEmision { get; set; } = DateTime.UtcNow;
    public decimal Subtotal { get; set; }
    public decimal Impuesto { get; set; }
    public decimal Total { get; set; }
    public string Estado { get; set; } = "emitida";
}
