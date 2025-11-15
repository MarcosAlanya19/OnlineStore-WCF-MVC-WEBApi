using Microsoft.AspNetCore.Mvc;
using TiendaOnline.MVC.Services;

namespace TiendaOnline.MVC.Controllers;

/// <summary>
/// Controlador MVC encargado de mostrar en la vista
/// el historial de auditorías de productos.
/// </summary>
public class AuditoriaController : Controller
{
    private readonly IAuditoriaService _auditoriaService;

    /// <summary>
    /// Inyección del servicio que consume la API de auditorías.
    /// </summary>
    /// <param name="auditoriaService">Servicio encargado de obtener datos de auditoría.</param>
    public AuditoriaController(IAuditoriaService auditoriaService)
    {
        _auditoriaService = auditoriaService;
    }

    /// <summary>
    /// Página principal de auditoría. Obtiene la lista desde el servicio
    /// y la envía a la vista para su renderizado.
    /// </summary>
    /// <returns>Vista con la lista de auditorías.</returns>
    public async Task<IActionResult> Index()
    {
        var auditorias = await _auditoriaService.GetAuditoriasAsync();
        return View(auditorias);
    }
}