using MensualidadesSerinsisPC.V2.Application.Abstractions;
using MensualidadesSerinsisPC.V2.Application.DTOs;
using MensualidadesSerinsisPC.V2.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MensualidadesSerinsisPC.V2.Infrastructure.Services;

public class LookupCatalogService(MensualidadesV2DbContext dbContext) : ILookupCatalogService
{
    public async Task<IReadOnlyList<LookupOptionDto>> GetCustomerOptionsAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Customers
            .OrderBy(x => x.CommercialName)
            .Select(x => new LookupOptionDto
            {
                Id = x.Id,
                Label = x.CommercialName
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<LookupOptionDto>> GetServicePlanOptionsAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.ServicePlans
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .Select(x => new LookupOptionDto
            {
                Id = x.Id,
                Label = x.Name
            })
            .ToListAsync(cancellationToken);
    }
}
