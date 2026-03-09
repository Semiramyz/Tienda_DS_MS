namespace Tienda_DS_MS.Server.Models.Ventas;

public class VentaDetalle
{
    public int Id { get; set; }
    public int VentaId { get; set; }
    public int ProductoId { get; set; }
    public string NombreProducto { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
    public Venta Venta { get; set; } = null!;
}
