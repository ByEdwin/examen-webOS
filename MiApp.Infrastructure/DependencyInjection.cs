// Aquí registro todos los servicios de la capa Infrastructure en el contenedor de dependencias
// Esto permite que las otras capas usen las interfaces sin conocer las implementaciones concretas
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiApp.Application.Interfaces;
using MiApp.Domain.Interfaces;
using MiApp.Infrastructure.Persistence;
using MiApp.Infrastructure.Persistence.Repositories;
using MiApp.Infrastructure.Services;

namespace MiApp.Infrastructure;

// Creo esta clase estática con un método de extensión para registrar los servicios
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuro Entity Framework para usar SQLite como base de datos
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        // Registro los repositorios: cuando alguien pida la interfaz, le doy la implementación concreta
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ITicketZoneRepository, TicketZoneRepository>();
        services.AddScoped<ITicketPurchaseRepository, TicketPurchaseRepository>();

        // Registro los servicios de autenticación
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();

        return services;
    }
}
