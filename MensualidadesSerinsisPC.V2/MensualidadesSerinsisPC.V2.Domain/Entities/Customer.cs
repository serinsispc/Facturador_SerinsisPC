using MensualidadesSerinsisPC.V2.Domain.Enums;

namespace MensualidadesSerinsisPC.V2.Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string CommercialName { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ServiceStatusType ServiceStatus { get; set; } = ServiceStatusType.GoodStanding;

    public ICollection<CustomerSubscription> Subscriptions { get; set; } = new List<CustomerSubscription>();
    public ICollection<CustomerDatabase> Databases { get; set; } = new List<CustomerDatabase>();
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
