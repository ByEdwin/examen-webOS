// Aquí defino el comando para cancelar un evento
// Solo necesito el ID del evento que quiero cancelar
using MediatR;

namespace MiApp.Application.Features.Events.Commands.CancelEvent;

// Creo un comando simple que solo recibe el ID del evento a cancelar
public record CancelEventCommand(int Id) : IRequest<CancelEventResponse>;

// Respuesta confirmando la cancelación
public record CancelEventResponse(string Message);
