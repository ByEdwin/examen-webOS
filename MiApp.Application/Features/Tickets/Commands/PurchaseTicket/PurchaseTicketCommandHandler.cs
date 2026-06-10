// Aquí implemento la lógica de compra de boletos
// Verifico disponibilidad, calculo el total y registro la compra
using MediatR;
using MiApp.Domain.Entities;
using MiApp.Domain.Enums;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.Tickets.Commands.PurchaseTicket;

// Creo el handler que procesa la compra de boletos
public class PurchaseTicketCommandHandler : IRequestHandler<PurchaseTicketCommand, PurchaseTicketResponse>
{
    // Inyecto los tres repositorios que necesito para procesar la compra
    private readonly IEventRepository _eventRepository;
    private readonly ITicketZoneRepository _ticketZoneRepository;
    private readonly ITicketPurchaseRepository _ticketPurchaseRepository;

    public PurchaseTicketCommandHandler(
        IEventRepository eventRepository,
        ITicketZoneRepository ticketZoneRepository,
        ITicketPurchaseRepository ticketPurchaseRepository)
    {
        _eventRepository = eventRepository;
        _ticketZoneRepository = ticketZoneRepository;
        _ticketPurchaseRepository = ticketPurchaseRepository;
    }

    public async Task<PurchaseTicketResponse> Handle(PurchaseTicketCommand request, CancellationToken cancellationToken)
    {
        // Primero verifico que el evento exista
        var evento = await _eventRepository.GetByIdAsync(request.EventId)
            ?? throw new KeyNotFoundException($"No encontré el evento con ID {request.EventId}.");

        // Verifico que el evento esté activo, no permito compras en eventos cancelados
        if (evento.Status == EventStatus.Cancelled)
            throw new InvalidOperationException("No se pueden comprar boletos para un evento cancelado.");

        // Busco la zona de boletaje seleccionada
        var zone = await _ticketZoneRepository.GetByIdAsync(request.TicketZoneId)
            ?? throw new KeyNotFoundException($"No encontré la zona de boletaje con ID {request.TicketZoneId}.");

        // Verifico que la zona pertenezca al evento seleccionado
        if (zone.EventId != request.EventId)
            throw new InvalidOperationException("La zona de boletaje no pertenece al evento seleccionado.");

        // Verifico que haya suficientes boletos disponibles
        if (zone.AvailableQuantity < request.Quantity)
            throw new InvalidOperationException(
                $"No hay suficientes boletos disponibles. Disponibles: {zone.AvailableQuantity}, Solicitados: {request.Quantity}.");

        // Calculo el total: Precio × Cantidad (según los requerimientos)
        var total = zone.Price * request.Quantity;

        // Creo el registro de la compra
        var purchase = new TicketPurchase
        {
            EventId = request.EventId,
            TicketZoneId = request.TicketZoneId,
            BuyerName = request.BuyerName,
            BuyerEmail = request.BuyerEmail,
            Quantity = request.Quantity,
            UnitPrice = zone.Price,       // Guardo el precio al momento de la compra
            Total = total,                // Total calculado
            PurchaseDate = DateTime.UtcNow
        };

        // Guardo la compra en la base de datos
        var created = await _ticketPurchaseRepository.AddAsync(purchase);

        // Descuento los boletos comprados de la disponibilidad de la zona
        zone.AvailableQuantity -= request.Quantity;
        await _ticketZoneRepository.UpdateAsync(zone);

        // Devuelvo los detalles de la compra al usuario
        return new PurchaseTicketResponse(
            created.Id,
            evento.Name,
            zone.Type.ToString(),
            request.Quantity,
            zone.Price,
            total,
            "¡Compra realizada exitosamente!"
        );
    }
}
