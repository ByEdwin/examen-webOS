// Aquí configuro el contexto de la base de datos usando Entity Framework Core
// Defino las tablas (DbSets) y las relaciones entre mis entidades
using Microsoft.EntityFrameworkCore;
using MiApp.Domain.Entities;
using MiApp.Domain.Enums;

namespace MiApp.Infrastructure.Persistence;

// Creo el DbContext que es el puente entre mi código C# y la base de datos SQLite
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Defino una tabla por cada entidad de mi dominio
    public DbSet<User> Users => Set<User>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<TicketZone> TicketZones => Set<TicketZone>();
    public DbSet<TicketPurchase> TicketPurchases => Set<TicketPurchase>();

    // Aquí configuro las relaciones y restricciones de cada tabla
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración de la tabla Users (ya existía)
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Role).HasConversion<string>().IsRequired();
        });

        // Configuración de la tabla Events
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Location).IsRequired().HasMaxLength(200);
            // Guardo el estado como string en la BD para que sea legible
            entity.Property(e => e.Status).HasConversion<string>().IsRequired();
            // Un evento tiene muchas zonas de boletaje
            entity.HasMany(e => e.TicketZones)
                  .WithOne(z => z.Event)
                  .HasForeignKey(z => z.EventId)
                  .OnDelete(DeleteBehavior.Cascade);
            // Un evento tiene muchas compras
            entity.HasMany(e => e.TicketPurchases)
                  .WithOne(p => p.Event)
                  .HasForeignKey(p => p.EventId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuración de la tabla TicketZones
        modelBuilder.Entity<TicketZone>(entity =>
        {
            entity.HasKey(z => z.Id);
            // Guardo el tipo de zona como string (VIP, Preferente, General)
            entity.Property(z => z.Type).HasConversion<string>().IsRequired();
            // El precio lo guardo con precisión decimal
            entity.Property(z => z.Price).HasColumnType("decimal(10,2)");
        });

        // Configuración de la tabla TicketPurchases
        modelBuilder.Entity<TicketPurchase>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.BuyerName).IsRequired().HasMaxLength(100);
            entity.Property(p => p.BuyerEmail).IsRequired().HasMaxLength(256);
            entity.Property(p => p.UnitPrice).HasColumnType("decimal(10,2)");
            entity.Property(p => p.Total).HasColumnType("decimal(10,2)");
            // Cada compra pertenece a una zona de boletaje
            entity.HasOne(p => p.TicketZone)
                  .WithMany()
                  .HasForeignKey(p => p.TicketZoneId)
                  .OnDelete(DeleteBehavior.Restrict);  // No elimino la zona si tiene compras
        });
    }
}
