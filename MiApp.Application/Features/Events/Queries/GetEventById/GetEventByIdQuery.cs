// Aquí defino la query para obtener el detalle de un evento específico por su ID
using MediatR;
using MiApp.Application.Features.Events.Queries.GetEvents;

namespace MiApp.Application.Features.Events.Queries.GetEventById;

// Creo la query que solo necesita el ID del evento
public record GetEventByIdQuery(int Id) : IRequest<EventDto>;
