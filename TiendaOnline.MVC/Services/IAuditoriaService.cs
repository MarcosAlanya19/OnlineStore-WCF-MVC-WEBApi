using TiendaOnline.MVC.Models;

namespace TiendaOnline.MVC.Services;

public interface IAuditoriaService
{
    Task<List<AuditoriaProducto>> GetAuditoriasAsync();

}