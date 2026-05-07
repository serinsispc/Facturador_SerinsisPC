using MensualidadesSerinsisPC.V2.Application.Abstractions;
using MensualidadesSerinsisPC.V2.Application.DTOs;
using MensualidadesSerinsisPC.V2.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MensualidadesSerinsisPC.V2.Infrastructure.Services;

public class IncomeReportingService(MensualidadesV2DbContext dbContext) : IIncomeReportingService
{
    public async Task<IReadOnlyList<MonthlyIncomeRowDto>> GetMonthlyIncomeAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT [Year], [Month], PaymentsCount, CustomersPaid, TotalIncome
            FROM dbo.V_MonthlyIncome
            ORDER BY [Year] DESC, [Month] DESC
            """;

        return await dbContext.Database.SqlQuery<MonthlyIncomeRowDto>($"{sql}")
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PortfolioSummaryRowDto>> GetPortfolioSummaryAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT [Year], [Month], InvoicesCount, TotalBilled, TotalPending
            FROM dbo.V_PortfolioSummary
            ORDER BY [Year] DESC, [Month] DESC
            """;

        return await dbContext.Database.SqlQuery<PortfolioSummaryRowDto>($"{sql}")
            .ToListAsync(cancellationToken);
    }
}
