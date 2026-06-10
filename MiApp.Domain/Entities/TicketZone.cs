// Aquí defino la entidad TicketZone que representa una zona de boletaje dentro de un evento
// Cada evento puede tener zonas como VIP, Preferente y General con precios diferentes
using MiApp.Domain.Enums;

namespace MiApp.Domain.Entities;

// Creo la clase TicketZone para manejar las diferentes zonas y sus precios por evento
public class TicketZone
{
    // Identificador único de la zona
    public int Id { get; set; }

    // Llave foránea que conecta esta zona con su evento
    public int EventId { get; set; }

    // Tipo de zona: VIP, Preferente o General
    // Uso un enum para restringir los valores posibles
    public ZoneType Type { get; set; }

    // Precio del boleto en esta zona, lo configura el administrador
    public decimal Price { get; set; }

    // Cantidad de boletos disponibles en esta zona
    // Se va decrementando conforme se compran boletos
    public int AvailableQuantity { get; set; }

    // Propiedad de navegación hacia el evento al que pertenece esta zona
    public Event Event { get; set; } = null!;
}
