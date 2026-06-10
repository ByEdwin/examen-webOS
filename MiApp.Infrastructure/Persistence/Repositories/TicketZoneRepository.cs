// Aquí implemento el repositorio de zonas de boletaje
// Lo uso principalmente para verificar disponibilidad y actualizar el stock de boletos
using Microsoft.EntityFrameworkCore;
using MiApp.Domain.Entities;
using MiApp.Domain.Interfaces;

namespace MiApp.Infrastructure.Persistence.Repositories;

// Creo la implementación concreta de ITicketZoneRepository
public class TicketZoneRepository : ITicketZoneRepository
{
    private readonly AppDbContext _context;

    public TicketZoneRepository(AppDbContext context)
    {
        _context = context;
    }

    // Obtengo una zona por su ID para verificar precios y disponibilidad
    public async Task<TicketZone?> GetByIdAsync(int id)
        => await _context.TicketZones.FirstOrDefaultAsync(z => z.Id == id);

    // Obtengo todas las zonas de un evento específico
    public async Task<IEnumerable<TicketZone>> GetByEventIdAsync(int eventId)
        => await _context.TicketZones.Where(z => z.EventId == eventId).ToListAsync();

    // Actualizo la zona (para decrementar la cantidad disponible después de una compra)
    public async Task UpdateAsync(TicketZone zone)
    {
        _context.TicketZones.Update(zone);
        await _context.SaveChangesAsync();
    }
}
