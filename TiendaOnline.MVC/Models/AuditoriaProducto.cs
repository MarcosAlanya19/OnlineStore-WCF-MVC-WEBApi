namespace TiendaOnline.MVC.Models;

public class AuditoriaProducto
{
public int IdAuditoria { get; set; }
public int IdProducto { get; set; }
public string Accion { get; set; }
public string NombreAnterior { get; set; }
public string NombreNuevo { get; set; }
public decimal? PrecioAnterior { get; set; }
public decimal? PrecioNuevo { get; set; }
public int? StockAnterior { get; set; }
public int? StockNuevo { get; set; }
public DateTime FechaAccion { get; set; }
}