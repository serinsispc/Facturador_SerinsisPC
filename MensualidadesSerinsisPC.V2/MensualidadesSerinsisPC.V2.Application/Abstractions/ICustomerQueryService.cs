using MensualidadesSerinsisPC.V2.Application.DTOs;

namespace MensualidadesSerinsisPC.V2.Application.Abstractions;

public interface ICustomerQueryService
{
    Task<IReadOnlyList<CustomerListItemDto>> GetCustomersAsync(CancellationToken cancellationToken = default);
}
