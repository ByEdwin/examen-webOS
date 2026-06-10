// Aquí manejo todas las excepciones de la aplicación de forma centralizada
// Capturo las excepciones y las convierto en respuestas HTTP con formato estándar ProblemDetails
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MiApp.API;

// Creo este handler global para que ninguna excepción llegue sin manejar al usuario
public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        // Determino el código HTTP y el título según el tipo de excepción
        var (statusCode, title) = exception switch
        {
            ValidationException => (StatusCodes.Status400BadRequest, "Error de validación"),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Recurso no encontrado"),
            InvalidOperationException => (StatusCodes.Status400BadRequest, "Operación no válida"),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "No autorizado"),
            _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor")
        };

        // Creo la respuesta con el formato estándar ProblemDetails
        var details = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception is ValidationException vex
                ? string.Join(" | ", vex.Errors.Select(e => e.ErrorMessage))
                : exception.Message
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(details, cancellationToken);
        return true;
    }
}
