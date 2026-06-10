// Aquí creo los datos iniciales (seed) para la base de datos
// Incluyo usuarios de prueba y eventos de ejemplo para que la app no inicie vacía
using MiApp.Application.Interfaces;
using MiApp.Domain.Entities;
using MiApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MiApp.Infrastructure.Persistence;

// Creo esta clase estática que se ejecuta al iniciar la aplicación
public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context, IPasswordHasher passwordHasher)
    {
        // Solo siembro datos si la base de datos está vacía (para no duplicar)
        if (await context.Users.AnyAsync()) return;

        // Creo los usuarios de prueba: un administrador y un lector
        var users = new List<User>
        {
            new()
            {
                Name = "Administrador",
                Email = "admin@miapp.com",
                PasswordHash = passwordHasher.Hash("Admin123!"),
                Role = Role.Admin,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "Lector Demo",
                Email = "reader@miapp.com",
                PasswordHash = passwordHasher.Hash("Reader123!"),
                Role = Role.Reader,
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Users.AddRange(users);

        // Creo eventos de ejemplo con sus zonas de boletaje para demostración
        var events = new List<Event>
        {
            new()
            {
                Name = "Concierto de Rock Nacional",
                Description = "Una noche épica con las mejores bandas de rock en español. Disfruta de 4 horas de música en vivo con artistas nacionales e internacionales.",
                Date = DateTime.UtcNow.AddDays(30),
                Location = "Auditorio Nacional, CDMX",
                Status = EventStatus.Active,
                CreatedAt = DateTime.UtcNow,
                TicketZones = new List<TicketZone>
                {
                    new() { Type = ZoneType.VIP, Price = 2500.00m, AvailableQuantity = 50 },
                    new() { Type = ZoneType.Preferente, Price = 1500.00m, AvailableQuantity = 150 },
                    new() { Type = ZoneType.General, Price = 800.00m, AvailableQuantity = 300 }
                }
            },
            new()
            {
                Name = "Conferencia Tech Summit 2026",
                Description = "El evento de tecnología más importante del año. Ponencias sobre IA, Cloud Computing y Desarrollo Web con speakers internacionales.",
                Date = DateTime.UtcNow.AddDays(45),
                Location = "Centro de Convenciones, Guadalajara",
                Status = EventStatus.Active,
                CreatedAt = DateTime.UtcNow,
                TicketZones = new List<TicketZone>
                {
                    new() { Type = ZoneType.VIP, Price = 5000.00m, AvailableQuantity = 30 },
                    new() { Type = ZoneType.Preferente, Price = 3000.00m, AvailableQuantity = 100 },
                    new() { Type = ZoneType.General, Price = 1500.00m, AvailableQuantity = 200 }
                }
            },
            new()
            {
                Name = "Obra de Teatro: El Fantasma",
                Description = "La aclamada obra de teatro llega a Monterrey. Una producción de primer nivel con actores reconocidos a nivel internacional.",
                Date = DateTime.UtcNow.AddDays(15),
                Location = "Teatro de la Ciudad, Monterrey",
                Status = EventStatus.Active,
                CreatedAt = DateTime.UtcNow,
                TicketZones = new List<TicketZone>
                {
                    new() { Type = ZoneType.VIP, Price = 1800.00m, AvailableQuantity = 40 },
                    new() { Type = ZoneType.Preferente, Price = 1200.00m, AvailableQuantity = 80 },
                    new() { Type = ZoneType.General, Price = 600.00m, AvailableQuantity = 200 }
                }
            }
        };

        context.Events.AddRange(events);
        await context.SaveChangesAsync();
    }
}
