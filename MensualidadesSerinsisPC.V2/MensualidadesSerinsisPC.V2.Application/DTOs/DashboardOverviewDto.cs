namespace MensualidadesSerinsisPC.V2.Application.DTOs;

public class DashboardOverviewDto
{
    public DashboardSummaryDto Summary { get; set; } = new();
    public int TotalCustomers { get; set; }
    public int PendingInvoices { get; set; }
    public int PendingNotifications { get; set; }
    public int ScheduledBillingsToday { get; set; }
    public List<DashboardDueItemDto> TopPendingCustomers { get; set; } = [];
    public List<DashboardNotificationItemDto> NotificationQueuePreview { get; set; } = [];
}
