using Microsoft.AspNetCore.Mvc;
using TiendaOnline.MVC.Models;
using TiendaOnline.MVC.Services;

namespace TiendaOnline.MVC.Controllers;

public class ProductosController : Controller
{
    private readonly IProductoService _productoService;

    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    // GET: Productos
    public async Task<IActionResult> Index()
    {
        var productos = await _productoService.GetProductosAsync();
        return View(productos);
    }

    // GET: Productos/Details/5
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

    // GET: Productos/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Productos/Create
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

    // GET: Productos/Edit/5
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

    // POST: Productos/Edit/5
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

    // GET: Productos/Delete/5
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

    // POST: Productos/Delete/5
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
