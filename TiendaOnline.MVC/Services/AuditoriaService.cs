using System.Text.Json;
using TiendaOnline.MVC.Models;

namespace TiendaOnline.MVC.Services;

public class AuditoriaService : IAuditoriaService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public AuditoriaService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5297");

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<List<AuditoriaProducto>> GetAuditoriasAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/auditoria");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("JSON recibido:");
                Console.WriteLine(content);
                return JsonSerializer.Deserialize<List<AuditoriaProducto>>(content, _jsonOptions) 
                       ?? new List<AuditoriaProducto>();
            }

            return new List<AuditoriaProducto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener auditorías: {ex.Message}");
            return new List<AuditoriaProducto>();
        }
    }
}
