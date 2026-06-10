// Aquí defino el comando para actualizar/editar un evento existente
// El administrador puede cambiar nombre, descripción, fecha, lugar y precios de las zonas
using MediatR;

namespace MiApp.Application.Features.Events.Commands.UpdateEvent;

// Creo el comando con todos los campos editables del evento
public record UpdateEventCommand(
    int Id,               // ID del evento que quiero editar
    string Name,          // Nuevo nombre
    string Description,   // Nueva descripción
    DateTime Date,        // Nueva fecha
    string Location,      // Nuevo lugar
    decimal VipPrice,     // Nuevo precio VIP
    int VipQuantity,      // Nueva cantidad VIP
    decimal PreferentePrice,  // Nuevo precio Preferente
    int PreferenteQuantity,   // Nueva cantidad Preferente
    decimal GeneralPrice,     // Nuevo precio General
    int GeneralQuantity       // Nueva cantidad General
) : IRequest<UpdateEventResponse>;

// Respuesta simple confirmando la actualización
public record UpdateEventResponse(string Message);
