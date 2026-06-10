// Aquí defino la entidad TicketPurchase que registra cada compra de boletos
// Guardo quién compró, cuántos boletos, en qué zona y el total calculado
namespace MiApp.Domain.Entities;

// Creo la clase TicketPurchase para llevar el registro de todas las compras
public class TicketPurchase
{
    // Identificador único de la compra
    public int Id { get; set; }

    // Llave foránea al evento donde se compraron los boletos
    public int EventId { get; set; }

    // Llave foránea a la zona de boletaje seleccionada (VIP, Preferente, General)
    public int TicketZoneId { get; set; }

    // Nombre del comprador para identificar quién hizo la compra
    public string BuyerName { get; set; } = string.Empty;

    // Email del comprador para enviarle confirmación
    public string BuyerEmail { get; set; } = string.Empty;

    // Cantidad de boletos comprados en esta transacción
    public int Quantity { get; set; }

    // Precio unitario al momento de la compra
    // Lo guardo por separado porque el precio de la zona puede cambiar después
    public decimal UnitPrice { get; set; }

    // Total de la compra: se calcula como Precio × Cantidad
    public decimal Total { get; set; }

    // Fecha y hora en que se realizó la compra
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

    // Propiedad de navegación hacia el evento
    public Event Event { get; set; } = null!;

    // Propiedad de navegación hacia la zona de boletaje
    public TicketZone TicketZone { get; set; } = null!;
}
