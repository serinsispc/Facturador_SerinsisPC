using MensualidadesSerinsisPC.V2.Domain.Enums;

namespace MensualidadesSerinsisPC.V2.Domain.Entities;

public class ServicePlan
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public BillingPeriodType BillingPeriod { get; set; } = BillingPeriodType.Monthly;
    public decimal BasePrice { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<CustomerSubscription> Subscriptions { get; set; } = new List<CustomerSubscription>();
}
