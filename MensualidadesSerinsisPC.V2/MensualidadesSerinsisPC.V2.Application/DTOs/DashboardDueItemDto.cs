namespace MensualidadesSerinsisPC.V2.Application.DTOs;

public class DashboardDueItemDto
{
    public string CustomerName { get; set; } = string.Empty;
    public string PlanName { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public decimal PendingAmount { get; set; }
    public string ServiceStatusName { get; set; } = string.Empty;
}
