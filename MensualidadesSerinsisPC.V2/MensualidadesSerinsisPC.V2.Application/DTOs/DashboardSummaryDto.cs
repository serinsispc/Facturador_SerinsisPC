namespace MensualidadesSerinsisPC.V2.Application.DTOs;

public class DashboardSummaryDto
{
    public int ActiveCustomers { get; set; }
    public int WarningCustomers { get; set; }
    public int SuspendedCustomers { get; set; }
    public decimal PendingPortfolio { get; set; }
    public decimal CurrentMonthIncome { get; set; }
}
