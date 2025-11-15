using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using TiendaOnline.gRPC.Data;
using TiendaOnline.gRPC.Models;

namespace TiendaOnline.gRPC.Services;

public class ProductosGrpcService: ProductosService.ProductosServiceBase
    {
        private readonly TiendaContext _context;
        private readonly ILogger<ProductosGrpcService> _logger;

        public ProductosGrpcService(TiendaContext context, ILogger<ProductosGrpcService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Obtener todos los productos
        public override async Task<ProductosResponse> GetProductos(Empty request, ServerCallContext context)
        {
            try
            {
                var productos = await _context.Productos.ToListAsync();
                var response = new ProductosResponse();

                foreach (var producto in productos)
                {
                    response.Productos.Add(new ProductoResponse
                    {
                        IdProducto = producto.IdProducto,
                        Nombre = producto.Nombre,
                        Descripcion = producto.Descripcion ?? "",
                        Precio = (double)producto.Precio,
                        Stock = producto.Stock,
                        Categoria = producto.Categoria ?? "",
                        Success = true,
                        Message = "OK"
                    });
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos");
                throw new RpcException(new Status(StatusCode.Internal, "Error al obtener productos"));
            }
        }

        // Obtener producto por ID
        public override async Task<ProductoResponse> GetProductoById(ProductoRequest request, ServerCallContext context)
        {
            try
            {
                var producto = await _context.Productos.FindAsync(request.Id);

                if (producto == null)
                {
                    return new ProductoResponse
                    {
                        Success = false,
                        Message = $"Producto con ID {request.Id} no encontrado"
                    };
                }

                return new ProductoResponse
                {
                    IdProducto = producto.IdProducto,
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion ?? "",
                    Precio = (double)producto.Precio,
                    Stock = producto.Stock,
                    Categoria = producto.Categoria ?? "",
                    Success = true,
                    Message = "Producto encontrado"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener producto por ID");
                throw new RpcException(new Status(StatusCode.Internal, "Error al obtener el producto"));
            }
        }

        // Crear nuevo producto
        public override async Task<ProductoResponse> CreateProducto(CreateProductoRequest request, ServerCallContext context)
        {
            try
            {
                var producto = new Producto()
                {
                    Nombre = request.Nombre,
                    Descripcion = request.Descripcion,
                    Precio = (decimal)request.Precio,
                    Stock = request.Stock,
                    Categoria = request.Categoria
                };

                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();

                return new ProductoResponse
                {
                    IdProducto = producto.IdProducto,
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion ?? "",
                    Precio = (double)producto.Precio,
                    Stock = producto.Stock,
                    Categoria = producto.Categoria ?? "",
                    Success = true,
                    Message = "Producto creado exitosamente"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear producto");
                throw new RpcException(new Status(StatusCode.Internal, "Error al crear el producto"));
            }
        }

        // Actualizar producto
        public override async Task<ProductoResponse> UpdateProducto(UpdateProductoRequest request, ServerCallContext context)
        {
            try
            {
                var producto = await _context.Productos.FindAsync(request.IdProducto);

                if (producto == null)
                {
                    return new ProductoResponse
                    {
                        Success = false,
                        Message = $"Producto con ID {request.IdProducto} no encontrado"
                    };
                }

                producto.Nombre = request.Nombre;
                producto.Descripcion = request.Descripcion;
                producto.Precio = (decimal)request.Precio;
                producto.Stock = request.Stock;
                producto.Categoria = request.Categoria;

                await _context.SaveChangesAsync();

                return new ProductoResponse
                {
                    IdProducto = producto.IdProducto,
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion ?? "",
                    Precio = (double)producto.Precio,
                    Stock = producto.Stock,
                    Categoria = producto.Categoria ?? "",
                    Success = true,
                    Message = "Producto actualizado exitosamente"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar producto");
                throw new RpcException(new Status(StatusCode.Internal, "Error al actualizar el producto"));
            }
        }

        // Eliminar producto
        public override async Task<DeleteResponse> DeleteProducto(ProductoRequest request, ServerCallContext context)
        {
            try
            {
                var producto = await _context.Productos.FindAsync(request.Id);

                if (producto == null)
                {
                    return new DeleteResponse
                    {
                        Success = false,
                        Message = $"Producto con ID {request.Id} no encontrado"
                    };
                }

                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();

                return new DeleteResponse
                {
                    Success = true,
                    Message = "Producto eliminado exitosamente"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar producto");
                throw new RpcException(new Status(StatusCode.Internal, "Error al eliminar el producto"));
            }
        }
    }