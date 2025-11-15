using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaOnline.WebAPI.Data;
using TiendaOnline.WebAPI.Models;

namespace TiendaOnline.WebAPI.Controllers;

/// <summary>
/// Controlador para la gestión de productos en la tienda online
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class ProductosController : ControllerBase
{
    private readonly TiendaContext _context;

    /// <summary>
    /// Constructor del controlador de productos
    /// </summary>
    /// <param name="context">Contexto de base de datos de la tienda</param>
    public ProductosController(TiendaContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene la lista completa de productos
    /// </summary>
    /// <returns>Lista de todos los productos disponibles</returns>
    /// <response code="200">Retorna la lista de productos exitosamente</response>
    /// <response code="500">Error interno del servidor al obtener productos</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Producto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Obtiene un producto específico por su ID
    /// </summary>
    /// <param name="id">ID del producto a buscar</param>
    /// <returns>Producto solicitado</returns>
    /// <response code="200">Retorna el producto encontrado</response>
    /// <response code="404">El producto no fue encontrado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Producto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Crea un nuevo producto en el inventario
    /// </summary>
    /// <param name="producto">Datos del producto a crear</param>
    /// <returns>Producto creado con su ID asignado</returns>
    /// <remarks>
    /// Ejemplo de request:
    /// 
    ///     POST /api/productos
    ///     {
    ///        "nombre": "Laptop HP",
    ///        "descripcion": "Laptop de alta gama",
    ///        "precio": 1299.99,
    ///        "stock": 10,
    ///        "categoria": "Electrónica"
    ///     }
    /// </remarks>
    /// <response code="201">Producto creado exitosamente</response>
    /// <response code="400">Datos del producto inválidos</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost]
    [ProducesResponseType(typeof(Producto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Actualiza un producto existente
    /// </summary>
    /// <param name="id">ID del producto a actualizar</param>
    /// <param name="producto">Datos actualizados del producto</param>
    /// <returns>Confirmación de actualización</returns>
    /// <remarks>
    /// El ID en la URL debe coincidir con el ID en el cuerpo del request.
    /// Este endpoint registra automáticamente los cambios en la tabla de auditoría.
    /// 
    /// Ejemplo de request:
    /// 
    ///     PUT /api/productos/5
    ///     {
    ///        "idProducto": 5,
    ///        "nombre": "Laptop HP Pro",
    ///        "descripcion": "Laptop profesional actualizada",
    ///        "precio": 1399.99,
    ///        "stock": 15,
    ///        "categoria": "Electrónica"
    ///     }
    /// </remarks>
    /// <response code="200">Producto actualizado exitosamente</response>
    /// <response code="400">El ID no coincide o datos inválidos</response>
    /// <response code="404">Producto no encontrado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutProducto(int id, Producto producto)
    {
        if (id != producto.IdProducto)
        {
            return BadRequest(new { message = "El ID del producto no coincide" });
        }

        try
        {
            // Cargar la entidad existente con tracking para auditoría
            var productoExistente = await _context.Productos.FindAsync(id);
        
            if (productoExistente == null)
            {
                return NotFound(new { message = $"Producto con ID {id} no encontrado" });
            }

            // Actualizar propiedades (EF Core detecta automáticamente los cambios)
            productoExistente.Nombre = producto.Nombre;
            productoExistente.Precio = producto.Precio;
            productoExistente.Stock = producto.Stock;
            productoExistente.Categoria = producto.Categoria;
            productoExistente.Descripcion = producto.Descripcion;

            // SaveChangesAsync ejecutará el override que registra la auditoría
            await _context.SaveChangesAsync();

            return Ok(new { message = "Producto actualizado exitosamente", producto = productoExistente });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al actualizar el producto", error = ex.Message });
        }
    }

    /// <summary>
    /// Elimina un producto del inventario
    /// </summary>
    /// <param name="id">ID del producto a eliminar</param>
    /// <returns>Confirmación de eliminación</returns>
    /// <remarks>
    /// Esta acción es irreversible, pero se registra en la tabla de auditoría.
    /// </remarks>
    /// <response code="200">Producto eliminado exitosamente</response>
    /// <response code="404">Producto no encontrado</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Verifica si un producto existe en la base de datos
    /// </summary>
    /// <param name="id">ID del producto a verificar</param>
    /// <returns>True si existe, False si no existe</returns>
    private bool ProductoExists(int id)
    {
        return _context.Productos.Any(e => e.IdProducto == id);
    }
}