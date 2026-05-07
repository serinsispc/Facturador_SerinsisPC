using MensualidadesSerinsisPC.V2.Application.DTOs;

namespace MensualidadesSerinsisPC.V2.Application.Abstractions;

public interface ICustomerCommandService
{
    Task<CustomerUpsertDto?> GetCustomerForEditAsync(int customerId, CancellationToken cancellationToken = default);
    Task<int> CreateCustomerAsync(CustomerUpsertDto request, CancellationToken cancellationToken = default);
    Task UpdateCustomerAsync(CustomerUpsertDto request, CancellationToken cancellationToken = default);
    Task UpdateServiceStatusAsync(int customerId, int serviceStatus, CancellationToken cancellationToken = default);
}
