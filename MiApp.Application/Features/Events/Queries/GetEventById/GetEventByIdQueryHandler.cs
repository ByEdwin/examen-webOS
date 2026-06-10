// Aquí implemento la lógica para obtener un evento específico por su ID
// Busco el evento y lo mapeo a un DTO con toda su información y zonas
using MediatR;
using MiApp.Application.Features.Events.Queries.GetEvents;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.Events.Queries.GetEventById;

// Creo el handler para obtener el detalle de un evento
public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, EventDto>
{
    private readonly IEventRepository _eventRepository;

    public GetEventByIdQueryHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<EventDto> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        // Busco el evento por ID, si no existe lanzo una excepción
        var evento = await _eventRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"No encontré el evento con ID {request.Id}.");

        // Mapeo la entidad a un DTO para devolver al controller
        return new EventDto(
            evento.Id,
            evento.Name,
            evento.Description,
            evento.Date,
            evento.Location,
            evento.Status.ToString(),
            evento.CreatedAt,
            evento.TicketZones.Select(z => new TicketZoneDto(
                z.Id,
                z.Type.ToString(),
                z.Price,
                z.AvailableQuantity
            )).ToList()
        );
    }
}
