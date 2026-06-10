// Aquí implemento la lógica para obtener la lista de eventos con filtros
// Uso el repositorio para buscar y luego mapeo las entidades a DTOs
using MediatR;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.Events.Queries.GetEvents;

// Creo el handler que procesa la consulta de eventos
public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, IEnumerable<EventDto>>
{
    private readonly IEventRepository _eventRepository;

    public GetEventsQueryHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<IEnumerable<EventDto>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        // Si hay filtros de búsqueda o fecha, uso el método Search del repositorio
        // Si no, obtengo todos los eventos o solo los activos según el parámetro
        IEnumerable<Domain.Entities.Event> events;

        if (!string.IsNullOrEmpty(request.Search) || request.FromDate.HasValue || request.ToDate.HasValue)
        {
            // Busco eventos con los filtros proporcionados
            events = await _eventRepository.SearchAsync(request.Search, request.FromDate, request.ToDate);
        }
        else if (request.IncludeCancelled)
        {
            // Para el admin: incluyo todos los eventos (activos y cancelados)
            events = await _eventRepository.GetAllAsync();
        }
        else
        {
            // Para el portal público: solo eventos activos
            events = await _eventRepository.GetActiveAsync();
        }

        // Mapeo las entidades a DTOs para no exponer las entidades del dominio directamente
        return events.Select(e => new EventDto(
            e.Id,
            e.Name,
            e.Description,
            e.Date,
            e.Location,
            e.Status.ToString(),
            e.CreatedAt,
            e.TicketZones.Select(z => new TicketZoneDto(
                z.Id,
                z.Type.ToString(),
                z.Price,
                z.AvailableQuantity
            )).ToList()
        ));
    }
}
