// Aquí implemento la lógica para actualizar un evento existente
// Busco el evento, actualizo sus datos y los de sus zonas de boletaje
using MediatR;
using MiApp.Domain.Enums;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.Events.Commands.UpdateEvent;

// Creo el handler que procesa la actualización del evento
public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, UpdateEventResponse>
{
    private readonly IEventRepository _eventRepository;

    public UpdateEventCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<UpdateEventResponse> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        // Busco el evento por su ID, si no existe lanzo una excepción
        var evento = await _eventRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"No encontré el evento con ID {request.Id}.");

        // Verifico que el evento no esté cancelado, no permito editar eventos cancelados
        if (evento.Status == EventStatus.Cancelled)
            throw new InvalidOperationException("No se puede editar un evento cancelado.");

        // Actualizo los datos básicos del evento
        evento.Name = request.Name;
        evento.Description = request.Description;
        evento.Date = request.Date;
        evento.Location = request.Location;

        // Actualizo los precios y cantidades de cada zona de boletaje
        foreach (var zone in evento.TicketZones)
        {
            switch (zone.Type)
            {
                case ZoneType.VIP:
                    zone.Price = request.VipPrice;
                    zone.AvailableQuantity = request.VipQuantity;
                    break;
                case ZoneType.Preferente:
                    zone.Price = request.PreferentePrice;
                    zone.AvailableQuantity = request.PreferenteQuantity;
                    break;
                case ZoneType.General:
                    zone.Price = request.GeneralPrice;
                    zone.AvailableQuantity = request.GeneralQuantity;
                    break;
            }
        }

        // Guardo los cambios en la base de datos
        await _eventRepository.UpdateAsync(evento);

        return new UpdateEventResponse("Evento actualizado exitosamente.");
    }
}
