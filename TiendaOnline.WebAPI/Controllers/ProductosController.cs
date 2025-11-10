using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaOnline.WebAPI.Data;
using TiendaOnline.WebAPI.Models;

namespace TiendaOnline.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductosController : ControllerBase
{
    private readonly TiendaContext _context;

    public ProductosController(TiendaContext context)
    {
        _context = context;
    }

    // GET: api/productos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
    {
        try
        {
            var productos = await _context.Productos.ToListAsync();
            return Ok(productos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener productos", error = ex.Message });
        }
    }

    // GET: api/productos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Producto>> GetProducto(int id)
    {
        try
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
            {
                return NotFound(new { message = $"Producto con ID {id} no encontrado" });
            }

            return Ok(producto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener el producto", error = ex.Message });
        }
    }

    // POST: api/productos
    [HttpPost]
    public async Task<ActionResult<Producto>> PostProducto(Producto producto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducto), new { id = producto.IdProducto }, producto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al crear el producto", error = ex.Message });
        }
    }

    // PUT: api/productos/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProducto(int id, Producto producto)
    {
        if (id != producto.IdProducto)
        {
            return BadRequest(new { message = "El ID del producto no coincide" });
        }

        try
        {
            _context.Entry(producto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Producto actualizado exitosamente", producto });
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductoExists(id))
            {
                return NotFound(new { message = $"Producto con ID {id} no encontrado" });
            }
            else
            {
                throw;
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al actualizar el producto", error = ex.Message });
        }
    }

    // DELETE: api/productos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProducto(int id)
    {
        try
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound(new { message = $"Producto con ID {id} no encontrado" });
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Producto eliminado exitosamente", id });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al eliminar el producto", error = ex.Message });
        }
    }

    private bool ProductoExists(int id)
    {
        return _context.Productos.Any(e => e.IdProducto == id);
    }
}