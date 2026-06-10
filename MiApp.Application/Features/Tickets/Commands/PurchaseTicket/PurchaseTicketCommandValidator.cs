// Aquí defino las validaciones para la compra de boletos
// Me aseguro de que todos los datos del comprador sean correctos antes de procesar
using FluentValidation;

namespace MiApp.Application.Features.Tickets.Commands.PurchaseTicket;

// Creo el validador que se ejecuta automáticamente antes del handler
public class PurchaseTicketCommandValidator : AbstractValidator<PurchaseTicketCommand>
{
    public PurchaseTicketCommandValidator()
    {
        // Valido que el ID del evento sea válido
        RuleFor(x => x.EventId)
            .GreaterThan(0).WithMessage("El ID del evento es inválido.");

        // Valido que el ID de la zona sea válido
        RuleFor(x => x.TicketZoneId)
            .GreaterThan(0).WithMessage("El ID de la zona es inválido.");

        // Valido que el nombre del comprador no esté vacío
        RuleFor(x => x.BuyerName)
            .NotEmpty().WithMessage("El nombre del comprador es requerido.")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres.");

        // Valido que el email del comprador sea válido
        RuleFor(x => x.BuyerEmail)
            .NotEmpty().WithMessage("El email del comprador es requerido.")
            .EmailAddress().WithMessage("El email no tiene un formato válido.");

        // Valido que la cantidad sea al menos 1 y máximo 10 boletos por compra
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("La cantidad debe ser al menos 1.")
            .LessThanOrEqualTo(10).WithMessage("No se pueden comprar más de 10 boletos por transacción.");
    }
}
