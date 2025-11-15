using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using TiendaOnline.gRPC.Data;
using TiendaOnline.gRPC.Models;

namespace TiendaOnline.gRPC.Services;

public class ProductosGrpcService: ProductosService.ProductosServiceBase
    {
        private readonly TiendaContext _context;
        private readonly ILogger<ProductosGrpcService> _logger;

        /// <summary>
        /// Inicializa el servicio gRPC de productos.
        /// </summary>
        public ProductosGrpcService(TiendaContext context, ILogger<ProductosGrpcService> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        /// <summary>
        /// Obtiene la lista completa de productos.
        /// </summary>
        /// <param name="request">Mensaje vacío requerido por gRPC.</param>
        /// <param name="context">Contexto de la llamada remota.</param>
        /// <returns>Listado de productos con estado y mensaje.</returns>
        /// <exception cref="RpcException">Error interno al consultar la base de datos.</exception>
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

        /// <summary>
        /// Obtiene un producto específico según su ID.
        /// </summary>
        /// <param name="request">Solicitud con el ID del producto.</param>
        /// <param name="context">Contexto de ejecución del servidor gRPC.</param>
        /// <returns>Datos del producto o un mensaje indicando que no fue encontrado.</returns>
        /// <exception cref="RpcException">Error interno al obtener el producto.</exception>
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

        /// <summary>
        /// Crea un nuevo producto en la base de datos.
        /// </summary>
        /// <param name="request">Datos del producto a registrar.</param>
        /// <param name="context">Contexto de la llamada gRPC.</param>
        /// <returns>Producto creado con estado y mensaje.</returns>
        /// <exception cref="RpcException">Error interno al guardar la información.</exception>
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

        /// <summary>
        /// Actualiza los datos de un producto existente.
        /// </summary>
        /// <param name="request">Información actualizada del producto.</param>
        /// <param name="context">Contexto de la llamada remota.</param>
        /// <returns>Producto actualizado o mensaje si no existe.</returns>
        /// <exception cref="RpcException">Error interno al actualizar el producto.</exception>
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

        /// <summary>
        /// Elimina un producto según su ID.
        /// </summary>
        /// <param name="request">Solicitud con el ID del producto.</param>
        /// <param name="context">Contexto de ejecución gRPC.</param>
        /// <returns>Resultado de la operación con mensaje.</returns>
        /// <exception cref="RpcException">Error interno al eliminar el producto.</exception>
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