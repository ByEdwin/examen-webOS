// Aquí implemento el repositorio de compras de boletos
// Lo uso para registrar compras y consultar datos para el dashboard de ventas
using Microsoft.EntityFrameworkCore;
using MiApp.Domain.Entities;
using MiApp.Domain.Interfaces;

namespace MiApp.Infrastructure.Persistence.Repositories;

// Creo la implementación concreta de ITicketPurchaseRepository
public class TicketPurchaseRepository : ITicketPurchaseRepository
{
    private readonly AppDbContext _context;

    public TicketPurchaseRepository(AppDbContext context)
    {
        _context = context;
    }

    // Registro una nueva compra de boletos en la base de datos
    public async Task<TicketPurchase> AddAsync(TicketPurchase purchase)
    {
        _context.TicketPurchases.Add(purchase);
        await _context.SaveChangesAsync();
        return purchase;
    }

    // Obtengo todas las compras incluyendo el evento y la zona (para el dashboard)
    // Uso Include para cargar las relaciones y poder acceder al nombre del evento y tipo de zona
    public async Task<IEnumerable<TicketPurchase>> GetAllAsync()
        => await _context.TicketPurchases
            .Include(p => p.Event)
            .Include(p => p.TicketZone)
            .OrderByDescending(p => p.PurchaseDate)
            .ToListAsync();

    // Obtengo las compras de un evento específico
    public async Task<IEnumerable<TicketPurchase>> GetByEventIdAsync(int eventId)
        => await _context.TicketPurchases
            .Include(p => p.TicketZone)
            .Where(p => p.EventId == eventId)
            .OrderByDescending(p => p.PurchaseDate)
            .ToListAsync();
}
