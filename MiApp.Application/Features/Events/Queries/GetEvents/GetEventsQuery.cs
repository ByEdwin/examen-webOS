// Aquí defino la query para obtener la lista de eventos
// Incluyo parámetros opcionales para filtrar y buscar eventos
using MediatR;

namespace MiApp.Application.Features.Events.Queries.GetEvents;

// Creo la query con filtros opcionales: búsqueda por texto, rango de fechas y si incluir cancelados
public record GetEventsQuery(
    string? Search,       // Texto para buscar en nombre o descripción
    DateTime? FromDate,   // Filtro de fecha desde
    DateTime? ToDate,     // Filtro de fecha hasta
    bool IncludeCancelled = false  // Si es true, incluyo eventos cancelados (para admin)
) : IRequest<IEnumerable<EventDto>>;

// Defino el DTO que devuelvo con los datos del evento y sus zonas
public record EventDto(
    int Id,
    string Name,
    string Description,
    DateTime Date,
    string Location,
    string Status,
    DateTime CreatedAt,
    List<TicketZoneDto> TicketZones
);

// DTO para las zonas de boletaje dentro del evento
public record TicketZoneDto(
    int Id,
    string Type,
    decimal Price,
    int AvailableQuantity
);
