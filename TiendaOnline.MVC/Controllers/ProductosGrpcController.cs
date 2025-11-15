using Microsoft.AspNetCore.Mvc;
using TiendaOnline.MVC.Models;
using TiendaOnline.MVC.Services;

namespace TiendaOnline.MVC.Controllers;

    public class ProductosGrpcController : Controller
    {
        private readonly IProductoGrpcService _productoGrpcService;

        public ProductosGrpcController(IProductoGrpcService productoGrpcService)
        {
            _productoGrpcService = productoGrpcService;
        }

        // GET: ProductosGrpc
        public async Task<IActionResult> Index()
        {
            var productos = await _productoGrpcService.GetProductosAsync();
            ViewBag.Source = "gRPC Service";
            return View("~/Views/Productos/Index.cshtml", productos);
        }

        // GET: ProductosGrpc/Details/5
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

        // GET: ProductosGrpc/Create
        public IActionResult Create()
        {
            ViewBag.Source = "gRPC Service";
            return View("~/Views/Productos/Create.cshtml");
        }

        // POST: ProductosGrpc/Create
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

        // GET: ProductosGrpc/Edit/5
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

        // POST: ProductosGrpc/Edit/5
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

        // GET: ProductosGrpc/Delete/5
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

        // POST: ProductosGrpc/Delete/5
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
