using MensualidadesSerinsisPC.V2.Application.DTOs;

namespace MensualidadesSerinsisPC.V2.Application.Abstractions;

public interface IIncomeReportingService
{
    Task<IReadOnlyList<MonthlyIncomeRowDto>> GetMonthlyIncomeAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PortfolioSummaryRowDto>> GetPortfolioSummaryAsync(CancellationToken cancellationToken = default);
}
