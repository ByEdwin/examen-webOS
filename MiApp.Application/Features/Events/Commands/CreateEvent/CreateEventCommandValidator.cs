// Aquí defino las validaciones para el comando de crear evento
// Uso FluentValidation para asegurarme de que los datos sean correctos antes de procesarlos
using FluentValidation;

namespace MiApp.Application.Features.Events.Commands.CreateEvent;

// Creo el validador que se ejecuta automáticamente antes del handler gracias al ValidationBehavior
public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        // Valido que el nombre del evento no esté vacío y no exceda 200 caracteres
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del evento es requerido.")
            .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres.");

        // Valido que la descripción esté presente
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripción del evento es requerida.");

        // Valido que la fecha del evento sea en el futuro
        RuleFor(x => x.Date)
            .GreaterThan(DateTime.UtcNow).WithMessage("La fecha del evento debe ser futura.");

        // Valido que el lugar no esté vacío
        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("El lugar del evento es requerido.");

        // Valido que los precios sean mayores a 0
        RuleFor(x => x.VipPrice)
            .GreaterThan(0).WithMessage("El precio VIP debe ser mayor a 0.");

        RuleFor(x => x.PreferentePrice)
            .GreaterThan(0).WithMessage("El precio Preferente debe ser mayor a 0.");

        RuleFor(x => x.GeneralPrice)
            .GreaterThan(0).WithMessage("El precio General debe ser mayor a 0.");

        // Valido que las cantidades sean al menos 1
        RuleFor(x => x.VipQuantity)
            .GreaterThan(0).WithMessage("La cantidad VIP debe ser al menos 1.");

        RuleFor(x => x.PreferenteQuantity)
            .GreaterThan(0).WithMessage("La cantidad Preferente debe ser al menos 1.");

        RuleFor(x => x.GeneralQuantity)
            .GreaterThan(0).WithMessage("La cantidad General debe ser al menos 1.");
    }
}
