// Aquí defino el enum EventStatus para controlar el estado de un evento
// Solo permito dos estados: Activo (se puede comprar) y Cancelado (ya no disponible)
namespace MiApp.Domain.Enums;

// Creo este enum para limitar los valores posibles del estado de un evento
public enum EventStatus
{
    // El evento está activo y disponible para compra de boletos
    Active,

    // El evento fue cancelado por el administrador
    Cancelled
}
