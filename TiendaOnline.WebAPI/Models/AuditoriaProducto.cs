using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaOnline.WebAPI.Models;

[Table("AuditoriaProductos")]
public class AuditoriaProducto
{
    [Key]
    public int IdAuditoria { get; set; }

    public int? IdProducto { get; set; }

    [MaxLength(20)]
    public string? Accion { get; set; }

    [MaxLength(100)]
    public string? NombreAnterior { get; set; }

    [MaxLength(100)]
    public string? NombreNuevo { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? PrecioAnterior { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? PrecioNuevo { get; set; }

    public int? StockAnterior { get; set; }

    public int? StockNuevo { get; set; }

    public DateTime FechaAccion { get; set; }
}