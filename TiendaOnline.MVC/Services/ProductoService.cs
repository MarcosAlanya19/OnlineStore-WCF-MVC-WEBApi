using System.Text;
using System.Text.Json;
using TiendaOnline.MVC.Models;

namespace TiendaOnline.MVC.Services;

public class ProductoService : IProductoService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ProductoService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configuration["ApiSettings:BaseUrl"] ?? "https://localhost:5297");
        
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<List<Producto>> GetProductosAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/productos");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Producto>>(content, _jsonOptions) ?? new List<Producto>();
            }
            
            return new List<Producto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener productos: {ex.Message}");
            return new List<Producto>();
        }
    }

    public async Task<Producto?> GetProductoByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/productos/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Producto>(content, _jsonOptions);
            }
            
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener producto: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> CreateProductoAsync(Producto producto)
    {
        try
        {
            var json = JsonSerializer.Serialize(producto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("/api/productos", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear producto: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateProductoAsync(int id, Producto producto)
    {
        try
        {
            var json = JsonSerializer.Serialize(producto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"/api/productos/{id}", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar producto: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteProductoAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/productos/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar producto: {ex.Message}");
            return false;
        }
    }
}
