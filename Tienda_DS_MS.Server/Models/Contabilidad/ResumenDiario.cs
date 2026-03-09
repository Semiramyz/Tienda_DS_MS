namespace Tienda_DS_MS.Server.Models.Contabilidad;

public class ResumenDiario
{
    public int Id { get; set; }
    public DateOnly Fecha { get; set; }
    public decimal TotalCompras { get; set; }
    public decimal TotalVentas { get; set; }
    public decimal Ganancia { get; set; }
}
