// Aquí implemento la lógica para cancelar un evento
// Cambio su estado a Cancelado para que ya no aparezca en el portal público
using MediatR;
using MiApp.Domain.Enums;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.Events.Commands.CancelEvent;

// Creo el handler que procesa la cancelación del evento
public class CancelEventCommandHandler : IRequestHandler<CancelEventCommand, CancelEventResponse>
{
    private readonly IEventRepository _eventRepository;

    public CancelEventCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<CancelEventResponse> Handle(CancelEventCommand request, CancellationToken cancellationToken)
    {
        // Busco el evento, si no existe lanzo error
        var evento = await _eventRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"No encontré el evento con ID {request.Id}.");

        // Verifico que no esté ya cancelado
        if (evento.Status == EventStatus.Cancelled)
            throw new InvalidOperationException("El evento ya está cancelado.");

        // Cambio el estado a Cancelado
        evento.Status = EventStatus.Cancelled;

        // Guardo el cambio en la base de datos
        await _eventRepository.UpdateAsync(evento);

        return new CancelEventResponse("Evento cancelado exitosamente.");
    }
}
