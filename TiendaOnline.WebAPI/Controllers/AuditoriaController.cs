using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaOnline.WebAPI.Data;
using TiendaOnline.WebAPI.Models;

namespace TiendaOnline.WebAPI.Controllers;

/// <summary>
/// Controlador encargado de exponer los endpoints relacionados
/// con la auditoría de productos dentro del sistema.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuditoriaController : ControllerBase
{
    private readonly TiendaContext _context;

    /// <summary>
    /// Inyecta el contexto de base de datos.
    /// </summary>
    /// <param name="context">Contexto de Entity Framework.</param>
    public AuditoriaController(TiendaContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene el historial de auditoría de productos, ordenado
    /// desde la acción más reciente hacia la más antigua.
    /// </summary>
    /// <returns>Lista de registros de auditoría.</returns>
    [HttpGet]
    public async Task<ActionResult<IList<AuditoriaProducto>>> GetAuditoria()
    {
        return await _context.AuditoriaProductos
            .OrderByDescending(a => a.FechaAccion)
            .ToListAsync();
    }
}