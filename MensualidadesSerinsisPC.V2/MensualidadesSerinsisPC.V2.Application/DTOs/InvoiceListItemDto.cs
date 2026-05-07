namespace MensualidadesSerinsisPC.V2.Application.DTOs;

public class InvoiceListItemDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int CustomerSubscriptionId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string PlanName { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PendingAmount { get; set; }
    public string InvoiceStatusName { get; set; } = string.Empty;
}
