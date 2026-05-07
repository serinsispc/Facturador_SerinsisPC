using MensualidadesSerinsisPC.V2.Application.DTOs;

namespace MensualidadesSerinsisPC.V2.Application.Abstractions;

public interface IDashboardService
{
    Task<DashboardOverviewDto> GetOverviewAsync(CancellationToken cancellationToken = default);
}
