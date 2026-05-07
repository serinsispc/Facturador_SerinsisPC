namespace MensualidadesSerinsisPC.V2.Application.DTOs;

public class MonthlyIncomeRowDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int PaymentsCount { get; set; }
    public int CustomersPaid { get; set; }
    public decimal TotalIncome { get; set; }
}
