using MensualidadesSerinsisPC.V2.Application.DTOs;

namespace MensualidadesSerinsisPC.V2.Application.Abstractions;

public interface ILookupCatalogService
{
    Task<IReadOnlyList<LookupOptionDto>> GetCustomerOptionsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LookupOptionDto>> GetServicePlanOptionsAsync(CancellationToken cancellationToken = default);
}
