using Microsoft.AspNetCore.Mvc;
using TiendaOnline.MVC.Services;

namespace TiendaOnline.MVC.Controllers;

public class AuditoriaController : Controller
{
    private readonly IAuditoriaService _auditoriaService;

    public AuditoriaController(IAuditoriaService auditoriaService)
    {
        _auditoriaService = auditoriaService;
    }

    public async Task<IActionResult> Index()
    {
        var auditorias = await _auditoriaService.GetAuditoriasAsync();
        return View(auditorias);
    }
}
