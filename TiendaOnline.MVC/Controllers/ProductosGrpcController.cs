using Microsoft.AspNetCore.Mvc;
using TiendaOnline.MVC.Models;
using TiendaOnline.MVC.Services;

namespace TiendaOnline.MVC.Controllers;

/// <summary>
/// Controlador MVC encargado de gestionar operaciones CRUD de productos
/// utilizando el servicio gRPC como fuente de datos.
/// </summary>
public class ProductosGrpcController : Controller
{
    private readonly IProductoGrpcService _productoGrpcService;

    /// <summary>
    /// Constructor que inyecta el servicio gRPC de productos.
    /// </summary>
    /// <param name="productoGrpcService">Servicio gRPC para operaciones de productos.</param>
    public ProductosGrpcController(IProductoGrpcService productoGrpcService)
    {
        _productoGrpcService = productoGrpcService;
    }

    /// <summary>
    /// Lista los productos obtenidos mediante gRPC.
    /// </summary>
    /// <returns>Vista con los productos.</returns>
    public async Task<IActionResult> Index()
    {
        var productos = await _productoGrpcService.GetProductosAsync();
        ViewBag.Source = "gRPC Service";
        return View("~/Views/Productos/Index.cshtml", productos);
    }

    /// <summary>
    /// Muestra los detalles de un producto específico desde gRPC.
    /// </summary>
    /// <param name="id">ID del producto.</param>
    /// <returns>Vista con los detalles del producto.</returns>
    public async Task<IActionResult> Details(int id)
    {
        var producto = await _productoGrpcService.GetProductoByIdAsync(id);

        if (producto == null)
        {
            TempData["Error"] = "Producto no encontrado (gRPC)";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Source = "gRPC Service";
        return View("~/Views/Productos/Details.cshtml", producto);
    }

    /// <summary>
    /// Muestra la vista para crear un nuevo producto mediante gRPC.
    /// </summary>
    /// <returns>Vista del formulario de creación.</returns>
    public IActionResult Create()
    {
        ViewBag.Source = "gRPC Service";
        return View("~/Views/Productos/Create.cshtml");
    }

    /// <summary>
    /// Procesa la creación de un producto mediante gRPC.
    /// </summary>
    /// <param name="producto">Datos del producto a crear.</param>
    /// <returns>Redirección o vista con errores.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Producto producto)
    {
        if (ModelState.IsValid)
        {
            var result = await _productoGrpcService.CreateProductoAsync(producto);

            if (result)
            {
                TempData["Success"] = "Producto creado exitosamente (vía gRPC)";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Error al crear el producto (gRPC)";
        }

        ViewBag.Source = "gRPC Service";
        return View("~/Views/Productos/Create.cshtml", producto);
    }

    /// <summary>
    /// Muestra la vista para editar un producto mediante gRPC.
    /// </summary>
    /// <param name="id">ID del producto.</param>
    /// <returns>Vista de edición.</returns>
    public async Task<IActionResult> Edit(int id)
    {
        var producto = await _productoGrpcService.GetProductoByIdAsync(id);

        if (producto == null)
        {
            TempData["Error"] = "Producto no encontrado (gRPC)";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Source = "gRPC Service";
        return View("~/Views/Productos/Edit.cshtml", producto);
    }

    /// <summary>
    /// Procesa la edición de un producto mediante gRPC.
    /// </summary>
    /// <param name="id">ID del producto.</param>
    /// <param name="producto">Datos actualizados del producto.</param>
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
            var result = await _productoGrpcService.UpdateProductoAsync(id, producto);

            if (result)
            {
                TempData["Success"] = "Producto actualizado exitosamente (vía gRPC)";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Error al actualizar el producto (gRPC)";
        }

        ViewBag.Source = "gRPC Service";
        return View("~/Views/Productos/Edit.cshtml", producto);
    }

    /// <summary>
    /// Muestra la vista de confirmación de eliminación utilizando gRPC.
    /// </summary>
    /// <param name="id">ID del producto.</param>
    /// <returns>Vista de eliminación.</returns>
    public async Task<IActionResult> Delete(int id)
    {
        var producto = await _productoGrpcService.GetProductoByIdAsync(id);

        if (producto == null)
        {
            TempData["Error"] = "Producto no encontrado (gRPC)";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Source = "gRPC Service";
        return View("~/Views/Productos/Delete.cshtml", producto);
    }

    /// <summary>
    /// Procesa la eliminación del producto mediante gRPC.
    /// </summary>
    /// <param name="id">ID del producto.</param>
    /// <returns>Redirección a la lista de productos.</returns>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _productoGrpcService.DeleteProductoAsync(id);

        if (result)
        {
            TempData["Success"] = "Producto eliminado exitosamente (vía gRPC)";
        }
        else
        {
            TempData["Error"] = "Error al eliminar el producto (gRPC)";
        }

        return RedirectToAction(nameof(Index));
    }
}