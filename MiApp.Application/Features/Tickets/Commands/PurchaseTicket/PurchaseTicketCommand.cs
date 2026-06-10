// Aquí defino el comando para comprar boletos
// El usuario selecciona un evento, una zona y la cantidad que desea comprar
using MediatR;

namespace MiApp.Application.Features.Tickets.Commands.PurchaseTicket;

// Creo el comando con los datos necesarios para procesar una compra de boletos
public record PurchaseTicketCommand(
    int EventId,        // ID del evento donde quiero comprar
    int TicketZoneId,   // ID de la zona de boletaje (VIP, Preferente o General)
    string BuyerName,   // Nombre del comprador
    string BuyerEmail,  // Email del comprador
    int Quantity        // Cantidad de boletos a comprar
) : IRequest<PurchaseTicketResponse>;

// Respuesta con los detalles de la compra realizada
public record PurchaseTicketResponse(
    int PurchaseId,     // ID de la compra generada
    string EventName,   // Nombre del evento
    string Zone,        // Zona seleccionada
    int Quantity,       // Cantidad comprada
    decimal UnitPrice,  // Precio unitario
    decimal Total,      // Total calculado: Precio × Cantidad
    string Message      // Mensaje de confirmación
);
