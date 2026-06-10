// Aquí defino la query para obtener el dashboard de ventas del administrador
// Devuelvo métricas como total de ventas, boletos vendidos, y ventas por zona
using MediatR;

namespace MiApp.Application.Features.Dashboard.Queries.GetDashboard;

// Creo la query sin parámetros porque el dashboard muestra toda la información general
public record GetDashboardQuery() : IRequest<DashboardDto>;

// Defino el DTO con todas las métricas del dashboard
public record DashboardDto(
    decimal TotalRevenue,           // Ingresos totales de todas las ventas
    int TotalTicketsSold,           // Total de boletos vendidos
    int TotalEvents,                // Total de eventos creados
    int ActiveEvents,               // Eventos activos actualmente
    int CancelledEvents,            // Eventos cancelados
    List<SalesByZoneDto> SalesByZone,       // Ventas agrupadas por tipo de zona
    List<RecentPurchaseDto> RecentPurchases  // Últimas 10 compras realizadas
);

// DTO para las ventas agrupadas por zona
public record SalesByZoneDto(
    string Zone,        // Nombre de la zona (VIP, Preferente, General)
    int TicketsSold,    // Boletos vendidos en esta zona
    decimal Revenue     // Ingresos de esta zona
);

// DTO para las compras recientes
public record RecentPurchaseDto(
    int Id,
    string EventName,
    string Zone,
    string BuyerName,
    int Quantity,
    decimal Total,
    DateTime PurchaseDate
);
