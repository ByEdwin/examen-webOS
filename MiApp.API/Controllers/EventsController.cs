// Aquí creo el controlador de eventos para el administrador
// Todos los endpoints requieren autenticación JWT con rol Admin
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiApp.Application.Features.Events.Commands.CreateEvent;
using MiApp.Application.Features.Events.Commands.UpdateEvent;
using MiApp.Application.Features.Events.Commands.CancelEvent;
using MiApp.Application.Features.Events.Queries.GetEvents;
using MiApp.Application.Features.Events.Queries.GetEventById;

namespace MiApp.API.Controllers;

// Protejo todo el controlador con autenticación JWT y rol Admin
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class EventsController : ControllerBase
{
    // Inyecto ISender de MediatR para enviar comandos y queries
    private readonly ISender _sender;

    public EventsController(ISender sender)
    {
        _sender = sender;
    }

    // POST /api/events → Crear un nuevo evento con sus zonas de boletaje
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventCommand command)
    {
        // Envío el comando a MediatR, que ejecuta el handler y las validaciones automáticamente
        var result = await _sender.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // PUT /api/events/{id} → Editar un evento existente
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEventCommand command)
    {
        // Me aseguro de que el ID de la URL coincida con el del body
        if (id != command.Id)
            return BadRequest(new { message = "El ID de la URL no coincide con el del cuerpo." });

        var result = await _sender.Send(command);
        return Ok(result);
    }

    // DELETE /api/events/{id} → Cancelar un evento (no lo borro, solo cambio el estado)
    [HttpDelete("{id}")]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await _sender.Send(new CancelEventCommand(id));
        return Ok(result);
    }

    // GET /api/events → Listar todos los eventos (incluye cancelados para el admin)
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        // Paso IncludeCancelled = true porque el admin necesita ver todos los eventos
        var result = await _sender.Send(new GetEventsQuery(search, from, to, IncludeCancelled: true));
        return Ok(result);
    }

    // GET /api/events/{id} → Obtener detalle de un evento específico
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _sender.Send(new GetEventByIdQuery(id));
        return Ok(result);
    }
}
