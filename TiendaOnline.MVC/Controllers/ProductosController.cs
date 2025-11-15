using Microsoft.AspNetCore.Mvc;
using TiendaOnline.MVC.Models;
using TiendaOnline.MVC.Services;

namespace TiendaOnline.MVC.Controllers;

/// <summary>
/// Controlador MVC encargado de gestionar las operaciones CRUD
/// relacionadas con los productos del sistema.
/// </summary>
public class ProductosController : Controller
{
    private readonly IProductoService _productoService;

    /// <summary>
    /// Constructor que inyecta el servicio de productos.
    /// </summary>
    /// <param name="productoService">Servicio encargado de la lógica de productos.</param>
    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    /// <summary>
    /// Lista todos los productos disponibles.
    /// </summary>
    /// <returns>Vista con la lista de productos.</returns>
    public async Task<IActionResult> Index()
    {
        var productos = await _productoService.GetProductosAsync();
        return View(productos);
    }

    /// <summary>
    /// Muestra los detalles de un producto según su ID.
    /// </summary>
    /// <param name="id">ID del producto.</param>
    /// <returns>Vista con los detalles del producto.</returns>
    public async Task<IActionResult> Details(int id)
    {
        var producto = await _productoService.GetProductoByIdAsync(id);

        if (producto == null)
        {
            TempData["Error"] = "Producto no encontrado";
            return RedirectToAction(nameof(Index));
        }

        return View(producto);
    }

    /// <summary>
    /// Página para la creación de un nuevo producto.
    /// </summary>
    /// <returns>Vista del formulario de creación.</returns>
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Procesa la creación de un nuevo producto.
    /// </summary>
    /// <param name="producto">Entidad del producto a crear.</param>
    /// <returns>Redirección o vista con errores.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Producto producto)
    {
        if (ModelState.IsValid)
        {
            var result = await _productoService.CreateProductoAsync(producto);

            if (result)
            {
                TempData["Success"] = "Producto creado exitosamente";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Error al crear el producto";
        }

        return View(producto);
    }

    /// <summary>
    /// Página para editar un producto existente.
    /// </summary>
    /// <param name="id">ID del producto.</param>
    /// <returns>Vista con los datos del producto.</returns>
    public async Task<IActionResult> Edit(int id)
    {
        var producto = await _productoService.GetProductoByIdAsync(id);

        if (producto == null)
        {
            TempData["Error"] = "Producto no encontrado";
            return RedirectToAction(nameof(Index));
        }

        return View(producto);
    }

    /// <summary>
    /// Procesa la edición de un producto.
    /// </summary>
    /// <param name="id">ID del producto.</param>
    /// <param name="producto">Datos actualizados.</param>
    /// <returns>Redirección o vista con errores.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Producto producto)
    {
        if (id != producto.IdProducto)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            var result = await _productoService.UpdateProductoAsync(id, producto);

            if (result)
            {
                TempData["Success"] = "Producto actualizado exitosamente";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Error al actualizar el producto";
        }

        return View(producto);
    }

    /// <summary>
    /// Página para confirmar la eliminación de un producto.
    /// </summary>
    /// <param name="id">ID del producto.</param>
    /// <returns>Vista de confirmación.</returns>
    public async Task<IActionResult> Delete(int id)
    {
        var producto = await _productoService.GetProductoByIdAsync(id);

        if (producto == null)
        {
            TempData["Error"] = "Producto no encontrado";
            return RedirectToAction(nameof(Index));
        }

        return View(producto);
    }

    /// <summary>
    /// Procesa la eliminación del producto.
    /// </summary>
    /// <param name="id">ID del producto a eliminar.</param>
    /// <returns>Redirección a la lista.</returns>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _productoService.DeleteProductoAsync(id);

        if (result)
        {
            TempData["Success"] = "Producto eliminado exitosamente";
        }
        else
        {
            TempData["Error"] = "Error al eliminar el producto";
        }

        return RedirectToAction(nameof(Index));
    }
}