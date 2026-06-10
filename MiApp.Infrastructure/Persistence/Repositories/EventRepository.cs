// Aquí implemento el repositorio de eventos que conecta con la base de datos SQLite
// Uso Entity Framework Core para las consultas y operaciones CRUD
using Microsoft.EntityFrameworkCore;
using MiApp.Domain.Entities;
using MiApp.Domain.Enums;
using MiApp.Domain.Interfaces;

namespace MiApp.Infrastructure.Persistence.Repositories;

// Creo la implementación concreta de IEventRepository
public class EventRepository : IEventRepository
{
    // Inyecto el contexto de la base de datos
    private readonly AppDbContext _context;

    public EventRepository(AppDbContext context)
    {
        _context = context;
    }

    // Obtengo un evento por su ID incluyendo sus zonas de boletaje
    public async Task<Event?> GetByIdAsync(int id)
        => await _context.Events
            .Include(e => e.TicketZones)  // Incluyo las zonas para tener los precios
            .FirstOrDefaultAsync(e => e.Id == id);

    // Obtengo todos los eventos con sus zonas (para el panel admin)
    public async Task<IEnumerable<Event>> GetAllAsync()
        => await _context.Events
            .Include(e => e.TicketZones)
            .OrderByDescending(e => e.CreatedAt)  // Los más recientes primero
            .ToListAsync();

    // Obtengo solo los eventos activos (para el portal público)
    public async Task<IEnumerable<Event>> GetActiveAsync()
        => await _context.Events
            .Include(e => e.TicketZones)
            .Where(e => e.Status == EventStatus.Active)
            .OrderBy(e => e.Date)  // Ordeno por fecha más próxima
            .ToListAsync();

    // Busco eventos con filtros opcionales de texto y rango de fechas
    public async Task<IEnumerable<Event>> SearchAsync(string? query, DateTime? from, DateTime? to)
    {
        // Inicio con todos los eventos activos
        var queryable = _context.Events
            .Include(e => e.TicketZones)
            .Where(e => e.Status == EventStatus.Active)
            .AsQueryable();

        // Si hay texto de búsqueda, filtro por nombre o descripción
        if (!string.IsNullOrWhiteSpace(query))
        {
            var search = query.ToLower();
            queryable = queryable.Where(e =>
                e.Name.ToLower().Contains(search) ||
                e.Description.ToLower().Contains(search) ||
                e.Location.ToLower().Contains(search));
        }

        // Si hay fecha desde, filtro eventos a partir de esa fecha
        if (from.HasValue)
            queryable = queryable.Where(e => e.Date >= from.Value);

        // Si hay fecha hasta, filtro eventos hasta esa fecha
        if (to.HasValue)
            queryable = queryable.Where(e => e.Date <= to.Value);

        return await queryable.OrderBy(e => e.Date).ToListAsync();
    }

    // Creo un nuevo evento y lo guardo en la base de datos
    public async Task<Event> AddAsync(Event entity)
    {
        _context.Events.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    // Actualizo un evento existente
    public async Task UpdateAsync(Event entity)
    {
        _context.Events.Update(entity);
        await _context.SaveChangesAsync();
    }
}
