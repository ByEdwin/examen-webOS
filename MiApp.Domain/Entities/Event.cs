// Aquí defino la entidad Event que representa un evento (concierto, conferencia, espectáculo)
// Esta clase pertenece a la capa Domain, que es el corazón de mi arquitectura limpia
using MiApp.Domain.Enums;

namespace MiApp.Domain.Entities;

// Creo la clase Event con todas las propiedades que necesito para describir un evento
public class Event
{
    // Identificador único del evento, lo uso como llave primaria en la base de datos
    public int Id { get; set; }

    // Nombre del evento, por ejemplo "Concierto de Rock"
    public string Name { get; set; } = string.Empty;

    // Descripción detallada del evento para que los usuarios sepan de qué se trata
    public string Description { get; set; } = string.Empty;

    // Fecha y hora en que se realizará el evento
    public DateTime Date { get; set; }

    // Lugar donde se llevará a cabo el evento
    public string Location { get; set; } = string.Empty;

    // Estado del evento: puede ser Activo o Cancelado
    // Por defecto lo inicio como Activo cuando se crea
    public EventStatus Status { get; set; } = EventStatus.Active;

    // Fecha de creación del registro, la asigno automáticamente
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relación uno a muchos: un evento tiene varias zonas de boletaje (VIP, Preferente, General)
    public List<TicketZone> TicketZones { get; set; } = new();

    // Relación uno a muchos: un evento tiene varias compras de boletos
    public List<TicketPurchase> TicketPurchases { get; set; } = new();
}
