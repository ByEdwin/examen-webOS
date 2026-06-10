// Aquí defino el enum ZoneType para los tipos de zona de boletaje
// Según los requerimientos, necesito tres tipos: VIP, Preferente y General
namespace MiApp.Domain.Enums;

// Creo este enum para restringir los tipos de zona permitidos
public enum ZoneType
{
    // Zona VIP: la más cara, mejor ubicación
    VIP,

    // Zona Preferente: precio intermedio, buena ubicación
    Preferente,

    // Zona General: la más económica, acceso básico
    General
}
