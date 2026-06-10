// Aquí defino las validaciones para editar un evento
// Reutilizo las mismas reglas que al crear, más la validación del ID
using FluentValidation;

namespace MiApp.Application.Features.Events.Commands.UpdateEvent;

// Creo el validador para el comando de actualización
public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
{
    public UpdateEventCommandValidator()
    {
        // El ID debe ser válido
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El ID del evento es inválido.");

        // Las mismas validaciones que al crear
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del evento es requerido.")
            .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripción del evento es requerida.");

        RuleFor(x => x.Date)
            .GreaterThan(DateTime.UtcNow).WithMessage("La fecha del evento debe ser futura.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("El lugar del evento es requerido.");

        RuleFor(x => x.VipPrice).GreaterThan(0).WithMessage("El precio VIP debe ser mayor a 0.");
        RuleFor(x => x.PreferentePrice).GreaterThan(0).WithMessage("El precio Preferente debe ser mayor a 0.");
        RuleFor(x => x.GeneralPrice).GreaterThan(0).WithMessage("El precio General debe ser mayor a 0.");
        RuleFor(x => x.VipQuantity).GreaterThan(0).WithMessage("La cantidad VIP debe ser al menos 1.");
        RuleFor(x => x.PreferenteQuantity).GreaterThan(0).WithMessage("La cantidad Preferente debe ser al menos 1.");
        RuleFor(x => x.GeneralQuantity).GreaterThan(0).WithMessage("La cantidad General debe ser al menos 1.");
    }
}
