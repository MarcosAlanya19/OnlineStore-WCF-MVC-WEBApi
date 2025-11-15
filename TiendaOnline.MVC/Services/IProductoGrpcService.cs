using TiendaOnline.MVC.Models;

namespace TiendaOnline.MVC.Services;

public interface IProductoGrpcService
{
    Task<List<Producto>> GetProductosAsync();
    Task<Producto?> GetProductoByIdAsync(int id);
    Task<bool> CreateProductoAsync(Producto producto);
    Task<bool> UpdateProductoAsync(int id, Producto producto);
    Task<bool> DeleteProductoAsync(int id);
}