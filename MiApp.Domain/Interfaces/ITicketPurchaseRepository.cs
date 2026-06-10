// Aquí defino la interfaz para registrar y consultar las compras de boletos
// La uso tanto para crear compras como para generar el dashboard de ventas
using MiApp.Domain.Entities;

namespace MiApp.Domain.Interfaces;

// Creo esta interfaz para las operaciones de compras de boletos
public interface ITicketPurchaseRepository
{
    // Registro una nueva compra de boletos
    Task<TicketPurchase> AddAsync(TicketPurchase purchase);

    // Obtengo todas las compras (para el dashboard de ventas del administrador)
    Task<IEnumerable<TicketPurchase>> GetAllAsync();

    // Obtengo las compras de un evento específico
    Task<IEnumerable<TicketPurchase>> GetByEventIdAsync(int eventId);
}
