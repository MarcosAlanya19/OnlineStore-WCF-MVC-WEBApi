using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaOnline.WebAPI.Data;
using TiendaOnline.WebAPI.Models;

namespace TiendaOnline.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuditoriaController : ControllerBase
{
    private readonly TiendaContext _context;

    public AuditoriaController(TiendaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IList<AuditoriaProducto>>> GetAuditoria()
    {
        return await _context.AuditoriaProductos
            .OrderByDescending(a => a.FechaAccion)
            .ToListAsync();
    }
}
