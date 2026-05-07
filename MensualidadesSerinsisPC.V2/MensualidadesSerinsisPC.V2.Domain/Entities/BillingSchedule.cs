namespace MensualidadesSerinsisPC.V2.Domain.Entities;

public class BillingSchedule
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int CustomerSubscriptionId { get; set; }
    public DateTime ScheduledDate { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public decimal AmountToBill { get; set; }
    public string ScheduleState { get; set; } = "Scheduled";
    public int? InvoiceId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
    public string LastMessage { get; set; } = string.Empty;
}
