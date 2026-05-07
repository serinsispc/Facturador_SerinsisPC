using MensualidadesSerinsisPC.V2.Domain.Enums;

namespace MensualidadesSerinsisPC.V2.Domain.Entities;

public class Invoice
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int CustomerSubscriptionId { get; set; }
    public DateTime IssuedAt { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PendingAmount { get; set; }
    public InvoiceStatusType Status { get; set; } = InvoiceStatusType.Issued;

    public Customer? Customer { get; set; }
    public CustomerSubscription? Subscription { get; set; }
    public ICollection<PaymentAllocation> PaymentAllocations { get; set; } = new List<PaymentAllocation>();
}
