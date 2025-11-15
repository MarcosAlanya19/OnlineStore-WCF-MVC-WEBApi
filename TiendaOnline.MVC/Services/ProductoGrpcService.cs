using Grpc.Net.Client;
using TiendaOnline.gRPC;
using TiendaOnline.MVC.Models;

namespace TiendaOnline.MVC.Services;

    public class ProductoGrpcService : IProductoGrpcService
    {
        private readonly GrpcChannel _channel;
        private readonly ProductosService.ProductosServiceClient _client;
        private readonly ILogger<ProductoGrpcService> _logger;

        public ProductoGrpcService(IConfiguration configuration, ILogger<ProductoGrpcService> logger)
        {
            _logger = logger;
            
            // Configurar el canal gRPC
            var grpcUrl = configuration["GrpcSettings:BaseUrl"] ?? "http://localhost:5123";
            
            // Configurar para ignorar certificados SSL en desarrollo
            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            _channel = GrpcChannel.ForAddress(grpcUrl, new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            });
            
            _client = new ProductosService.ProductosServiceClient(_channel);
        }

        public async Task<List<Producto>> GetProductosAsync()
        {
            try
            {
                var response = await _client.GetProductosAsync(new Empty());
                
                return response.Productos.Select(p => new Producto
                {
                    IdProducto = p.IdProducto,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = (decimal)p.Precio,
                    Stock = p.Stock,
                    Categoria = p.Categoria
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos desde gRPC");
                return new List<Producto>();
            }
        }

        public async Task<Producto?> GetProductoByIdAsync(int id)
        {
            try
            {
                var response = await _client.GetProductoByIdAsync(new ProductoRequest { Id = id });
                
                if (!response.Success)
                {
                    return null;
                }

                return new Producto
                {
                    IdProducto = response.IdProducto,
                    Nombre = response.Nombre,
                    Descripcion = response.Descripcion,
                    Precio = (decimal)response.Precio,
                    Stock = response.Stock,
                    Categoria = response.Categoria
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener producto por ID desde gRPC");
                return null;
            }
        }

        public async Task<bool> CreateProductoAsync(Producto producto)
        {
            try
            {
                var request = new CreateProductoRequest
                {
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion ?? "",
                    Precio = (double)producto.Precio,
                    Stock = producto.Stock,
                    Categoria = producto.Categoria ?? ""
                };

                var response = await _client.CreateProductoAsync(request);
                return response.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear producto desde gRPC");
                return false;
            }
        }

        public async Task<bool> UpdateProductoAsync(int id, Producto producto)
        {
            try
            {
                var request = new UpdateProductoRequest
                {
                    IdProducto = id,
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion ?? "",
                    Precio = (double)producto.Precio,
                    Stock = producto.Stock,
                    Categoria = producto.Categoria ?? ""
                };

                var response = await _client.UpdateProductoAsync(request);
                return response.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar producto desde gRPC");
                return false;
            }
        }

        public async Task<bool> DeleteProductoAsync(int id)
        {
            try
            {
                var response = await _client.DeleteProductoAsync(new ProductoRequest { Id = id });
                return response.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar producto desde gRPC");
                return false;
            }
        }
    
}