// Aquí implemento la lógica para calcular todas las métricas del dashboard de ventas
// Consulto las compras y eventos para generar estadísticas útiles para el administrador
using MediatR;
using MiApp.Domain.Enums;
using MiApp.Domain.Interfaces;

namespace MiApp.Application.Features.Dashboard.Queries.GetDashboard;

// Creo el handler que calcula todas las métricas del dashboard
public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, DashboardDto>
{
    private readonly IEventRepository _eventRepository;
    private readonly ITicketPurchaseRepository _purchaseRepository;

    public GetDashboardQueryHandler(
        IEventRepository eventRepository,
        ITicketPurchaseRepository purchaseRepository)
    {
        _eventRepository = eventRepository;
        _purchaseRepository = purchaseRepository;
    }

    public async Task<DashboardDto> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        // Obtengo todos los eventos y todas las compras para calcular las métricas
        var events = (await _eventRepository.GetAllAsync()).ToList();
        var purchases = (await _purchaseRepository.GetAllAsync()).ToList();

        // Calculo el total de ingresos sumando el total de todas las compras
        var totalRevenue = purchases.Sum(p => p.Total);

        // Cuento el total de boletos vendidos
        var totalTicketsSold = purchases.Sum(p => p.Quantity);

        // Cuento los eventos por estado
        var totalEvents = events.Count;
        var activeEvents = events.Count(e => e.Status == EventStatus.Active);
        var cancelledEvents = events.Count(e => e.Status == EventStatus.Cancelled);

        // Agrupo las ventas por tipo de zona para ver cuál genera más ingresos
        var salesByZone = purchases
            .GroupBy(p => p.TicketZone?.Type.ToString() ?? "Desconocido")
            .Select(g => new SalesByZoneDto(
                g.Key,
                g.Sum(p => p.Quantity),
                g.Sum(p => p.Total)
            ))
            .ToList();

        // Obtengo las últimas 10 compras para mostrar la actividad reciente
        var recentPurchases = purchases
            .OrderByDescending(p => p.PurchaseDate)
            .Take(10)
            .Select(p => new RecentPurchaseDto(
                p.Id,
                p.Event?.Name ?? "Evento desconocido",
                p.TicketZone?.Type.ToString() ?? "Zona desconocida",
                p.BuyerName,
                p.Quantity,
                p.Total,
                p.PurchaseDate
            ))
            .ToList();

        // Devuelvo todas las métricas en un solo DTO
        return new DashboardDto(
            totalRevenue,
            totalTicketsSold,
            totalEvents,
            activeEvents,
            cancelledEvents,
            salesByZone,
            recentPurchases
        );
    }
}
