using MensualidadesSerinsisPC.V2.Domain.Enums;

namespace MensualidadesSerinsisPC.V2.Domain.Entities;

public class CustomerSubscription
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int ServicePlanId { get; set; }
    public DateTime StartDate { get; set; }
    public int PaymentDay { get; set; }
    public DateTime NextBillingDate { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public int GraceDays { get; set; } = 5;
    public bool AutomaticBillingEnabled { get; set; } = true;
    public bool AutomaticCollectionEnabled { get; set; } = true;
    public ServiceStatusType ServiceStatus { get; set; } = ServiceStatusType.GoodStanding;

    public Customer? Customer { get; set; }
    public ServicePlan? ServicePlan { get; set; }
}
