// Aquí implemento el handler que procesa el comando de crear evento
// Este handler contiene la lógica de negocio para crear el evento y sus zonas de boletaje
using MediatR;
using MiApp.Domain.Entities;
using MiApp.Domain.Enums;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.Events.Commands.CreateEvent;

// Creo el handler que MediatR ejecutará cuando reciba un CreateEventCommand
public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, CreateEventResponse>
{
    // Inyecto el repositorio de eventos para guardar en la base de datos
    private readonly IEventRepository _eventRepository;

    public CreateEventCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    // Este método se ejecuta cuando el Controller envía el comando a través de MediatR
    public async Task<CreateEventResponse> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        // Creo la entidad Event con los datos del comando
        var evento = new Event
        {
            Name = request.Name,
            Description = request.Description,
            Date = request.Date,
            Location = request.Location,
            Status = EventStatus.Active,  // Todo evento nuevo inicia como Activo
            CreatedAt = DateTime.UtcNow,
            // Creo las tres zonas de boletaje con los precios que configuró el administrador
            TicketZones = new List<TicketZone>
            {
                new() { Type = ZoneType.VIP, Price = request.VipPrice, AvailableQuantity = request.VipQuantity },
                new() { Type = ZoneType.Preferente, Price = request.PreferentePrice, AvailableQuantity = request.PreferenteQuantity },
                new() { Type = ZoneType.General, Price = request.GeneralPrice, AvailableQuantity = request.GeneralQuantity }
            }
        };

        // Guardo el evento en la base de datos a través del repositorio
        var created = await _eventRepository.AddAsync(evento);

        // Devuelvo el ID del evento creado y un mensaje de confirmación
        return new CreateEventResponse(created.Id, "Evento creado exitosamente.");
    }
}
