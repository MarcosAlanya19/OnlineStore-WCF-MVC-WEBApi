using Microsoft.EntityFrameworkCore;
using TiendaOnline.gRPC.Data;
using TiendaOnline.gRPC.Services;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios gRPC
builder.Services.AddGrpc();

// Configurar Entity Framework con SQL Server
builder.Services.AddDbContext<TiendaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Mapear servicios gRPC
app.MapGrpcService<GreeterService>();
app.MapGrpcService<ProductosGrpcService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();