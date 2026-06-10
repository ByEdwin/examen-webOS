// Aquí creo el controlador público que no requiere autenticación
// Los usuarios pueden ver eventos activos y comprar boletos sin necesidad de login
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiApp.Application.Features.Events.Queries.GetEvents;
using MiApp.Application.Features.Events.Queries.GetEventById;
using MiApp.Application.Features.Tickets.Commands.PurchaseTicket;

namespace MiApp.API.Controllers;

// Este controlador es público, no tiene [Authorize] para que cualquier usuario pueda acceder
[ApiController]
[Route("api/[controller]")]
public class PublicController : ControllerBase
{
    // Inyecto ISender de MediatR para enviar queries y comandos
    private readonly ISender _sender;

    public PublicController(ISender sender)
    {
        _sender = sender;
    }

    // GET /api/public/events → Listar eventos activos con filtros de búsqueda
    // Los usuarios pueden buscar por nombre, descripción o lugar, y filtrar por fechas
    [HttpGet("events")]
    public async Task<IActionResult> GetEvents(
        [FromQuery] string? search,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
    {
        // IncludeCancelled = false porque los usuarios solo deben ver eventos activos
        var result = await _sender.Send(new GetEventsQuery(search, from, to, IncludeCancelled: false));
        return Ok(result);
    }

    // GET /api/public/events/{id} → Ver detalle de un evento con sus zonas y precios
    [HttpGet("events/{id}")]
    public async Task<IActionResult> GetEventById(int id)
    {
        var result = await _sender.Send(new GetEventByIdQuery(id));
        return Ok(result);
    }

    // POST /api/public/purchase → Comprar boletos para un evento
    // El usuario selecciona el evento, la zona y la cantidad
    [HttpPost("purchase")]
    public async Task<IActionResult> Purchase([FromBody] PurchaseTicketCommand command)
    {
        // MediatR ejecuta las validaciones y luego el handler de compra
        var result = await _sender.Send(command);
        return Ok(result);
    }
}
