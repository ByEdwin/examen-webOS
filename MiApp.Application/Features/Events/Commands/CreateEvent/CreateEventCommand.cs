// Aquí defino el comando para crear un nuevo evento
// Este record actúa como un mensaje que le dice a MediatR "quiero crear un evento con estos datos"
using MediatR;

namespace MiApp.Application.Features.Events.Commands.CreateEvent;

// Creo el comando como un record inmutable con los datos necesarios para crear un evento
// Implemento IRequest<CreateEventResponse> para que MediatR sepa qué tipo de respuesta devolver
public record CreateEventCommand(
    string Name,          // Nombre del evento
    string Description,   // Descripción del evento
    DateTime Date,        // Fecha del evento
    string Location,      // Lugar del evento
    decimal VipPrice,     // Precio de la zona VIP
    int VipQuantity,      // Cantidad de boletos VIP
    decimal PreferentePrice,  // Precio de la zona Preferente
    int PreferenteQuantity,   // Cantidad de boletos Preferente
    decimal GeneralPrice,     // Precio de la zona General
    int GeneralQuantity       // Cantidad de boletos General
) : IRequest<CreateEventResponse>;

// Defino la respuesta que devuelvo después de crear el evento exitosamente
public record CreateEventResponse(int Id, string Message);
