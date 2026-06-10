// Aquí defino la interfaz para acceder a las zonas de boletaje
// La necesito para consultar y actualizar la disponibilidad de boletos por zona
using MiApp.Domain.Entities;

namespace MiApp.Domain.Interfaces;

// Creo esta interfaz para las operaciones de zonas de boletaje
public interface ITicketZoneRepository
{
    // Obtengo una zona por su ID para verificar disponibilidad antes de una compra
    Task<TicketZone?> GetByIdAsync(int id);

    // Obtengo todas las zonas de un evento específico
    Task<IEnumerable<TicketZone>> GetByEventIdAsync(int eventId);

    // Actualizo la zona (principalmente para decrementar la cantidad disponible)
    Task UpdateAsync(TicketZone zone);
}
