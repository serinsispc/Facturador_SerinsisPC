namespace MensualidadesSerinsisPC.V2.Application.DTOs;

public class SubscriptionListItemDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int ServicePlanId { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public int BillingPeriodMonths { get; set; }
    public DateTime StartDate { get; set; }
    public int PaymentDay { get; set; }
    public DateTime NextBillingDate { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public int GraceDays { get; set; }
    public bool AutomaticBillingEnabled { get; set; }
    public bool AutomaticCollectionEnabled { get; set; }
    public string ServiceStatusName { get; set; } = string.Empty;
}
