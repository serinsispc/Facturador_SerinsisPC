using System.ComponentModel.DataAnnotations;

namespace MensualidadesSerinsisPC.V2.Application.DTOs;

public class SubscriptionUpsertDto
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un cliente.")]
    public int CustomerId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un plan.")]
    public int ServicePlanId { get; set; }

    [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [Range(1, 31, ErrorMessage = "El dia de pago debe estar entre 1 y 31.")]
    public int PaymentDay { get; set; } = 5;

    [Required(ErrorMessage = "La proxima fecha de cobro es obligatoria.")]
    public DateTime NextBillingDate { get; set; } = DateTime.Today;

    public DateTime? LastPaymentDate { get; set; }

    [Range(0, 30, ErrorMessage = "Los dias de gracia deben estar entre 0 y 30.")]
    public int GraceDays { get; set; } = 5;

    public bool AutomaticBillingEnabled { get; set; } = true;
    public bool AutomaticCollectionEnabled { get; set; } = true;
}
