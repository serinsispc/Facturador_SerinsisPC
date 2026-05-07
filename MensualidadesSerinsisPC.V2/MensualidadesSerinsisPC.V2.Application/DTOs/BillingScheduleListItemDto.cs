namespace MensualidadesSerinsisPC.V2.Application.DTOs;

public class BillingScheduleListItemDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int CustomerSubscriptionId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string PlanName { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public decimal AmountToBill { get; set; }
    public string ScheduleState { get; set; } = string.Empty;
    public int? InvoiceId { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string LastMessage { get; set; } = string.Empty;
}
