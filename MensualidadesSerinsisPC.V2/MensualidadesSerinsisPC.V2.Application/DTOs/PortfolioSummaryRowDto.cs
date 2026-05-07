namespace MensualidadesSerinsisPC.V2.Application.DTOs;

public class PortfolioSummaryRowDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int InvoicesCount { get; set; }
    public decimal TotalBilled { get; set; }
    public decimal TotalPending { get; set; }
}
