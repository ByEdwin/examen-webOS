// Aquí defino la interfaz IEventRepository que establece el contrato para acceder a los eventos
// La implementación real estará en la capa Infrastructure, así mantengo el Domain independiente
using MiApp.Domain.Entities;

namespace MiApp.Domain.Interfaces;

// Creo esta interfaz siguiendo el principio de inversión de dependencias (la D de SOLID)
// La capa Domain define QUÉ necesita, y la capa Infrastructure define CÓMO lo hace
public interface IEventRepository
{
    // Obtengo un evento por su ID, incluyendo sus zonas de boletaje
    Task<Event?> GetByIdAsync(int id);

    // Obtengo todos los eventos (para el panel de administración)
    Task<IEnumerable<Event>> GetAllAsync();

    // Obtengo solo los eventos activos (para el portal público)
    Task<IEnumerable<Event>> GetActiveAsync();

    // Busco eventos con filtros opcionales: texto de búsqueda y rango de fechas
    Task<IEnumerable<Event>> SearchAsync(string? query, DateTime? from, DateTime? to);

    // Creo un nuevo evento y lo guardo en la base de datos
    Task<Event> AddAsync(Event entity);

    // Actualizo un evento existente
    Task UpdateAsync(Event entity);
}
