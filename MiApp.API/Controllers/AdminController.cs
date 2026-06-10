// Aquí actualizo el controlador de administración con el dashboard de ventas real
// Mantengo la funcionalidad de listar usuarios y agrego el dashboard con métricas
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiApp.Domain.Interfaces;
using MiApp.Application.Features.Dashboard.Queries.GetDashboard;

namespace MiApp.API.Controllers;

// Protejo este controlador para que solo los administradores puedan acceder
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ISender _sender;

    // Inyecto el repositorio de usuarios y MediatR
    public AdminController(IUserRepository userRepository, ISender sender)
    {
        _userRepository = userRepository;
        _sender = sender;
    }

    // GET /api/admin/users → Listar todos los usuarios del sistema
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userRepository.GetAllAsync();
        // Proyecto solo los campos que necesito, sin exponer el password hash
        var result = users.Select(u => new { u.Id, u.Name, u.Email, Role = u.Role.ToString(), u.CreatedAt });
        return Ok(result);
    }

    // GET /api/admin/dashboard → Dashboard de ventas con métricas reales
    // Aquí uso MediatR para obtener las estadísticas calculadas en el handler
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var result = await _sender.Send(new GetDashboardQuery());
        return Ok(result);
    }
}
