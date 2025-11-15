using Microsoft.EntityFrameworkCore;
using TiendaOnline.WebAPI.Models;

namespace TiendaOnline.WebAPI.Data;

public class TiendaContext : DbContext
{
    public TiendaContext(DbContextOptions<TiendaContext> options) : base(options)
    {
    }
    
    public DbSet<AuditoriaProducto> AuditoriaProductos { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var cambios = ChangeTracker.Entries<Producto>()
            .Where(e => e.State == EntityState.Added 
                        || e.State == EntityState.Modified 
                        || e.State == EntityState.Deleted)
            .ToList();

        foreach (var entry in cambios)
        {
            var auditoria = new AuditoriaProducto
            {
                IdProducto = entry.Entity.IdProducto,
                FechaAccion = DateTime.UtcNow
            };

            switch (entry.State)
            {
                case EntityState.Added:
                    auditoria.Accion = "INSERT";
                    auditoria.NombreNuevo = entry.Entity.Nombre;
                    auditoria.PrecioNuevo = entry.Entity.Precio;
                    auditoria.StockNuevo = entry.Entity.Stock;
                    break;

                case EntityState.Modified:
                    auditoria.Accion = "UPDATE";
                    auditoria.NombreAnterior = entry.OriginalValues.GetValue<string>("Nombre");
                    auditoria.NombreNuevo = entry.CurrentValues.GetValue<string>("Nombre");
                    auditoria.PrecioAnterior = entry.OriginalValues.GetValue<decimal>("Precio");
                    auditoria.PrecioNuevo = entry.CurrentValues.GetValue<decimal>("Precio");
                    auditoria.StockAnterior = entry.OriginalValues.GetValue<int>("Stock");
                    auditoria.StockNuevo = entry.CurrentValues.GetValue<int>("Stock");
                    break;

                case EntityState.Deleted:
                    auditoria.Accion = "DELETE";
                    auditoria.NombreAnterior = entry.OriginalValues.GetValue<string>("Nombre");
                    auditoria.PrecioAnterior = entry.OriginalValues.GetValue<decimal>("Precio");
                    auditoria.StockAnterior = entry.OriginalValues.GetValue<int>("Stock");
                    break;
            }

            AuditoriaProductos.Add(auditoria);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    public DbSet<Producto> Productos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto);
            entity.Property(e => e.Precio).HasPrecision(10, 2);
        });
    }
}