using MensualidadesSerinsisPC.V2.Application.DTOs;

namespace MensualidadesSerinsisPC.V2.Application.Abstractions;

public interface ICustomerDatabaseService
{
    Task<IReadOnlyList<CustomerDatabaseListItemDto>> GetDatabasesAsync(CancellationToken cancellationToken = default);
    Task<CustomerDatabaseUpsertDto?> GetDatabaseForEditAsync(int databaseId, CancellationToken cancellationToken = default);
    Task<int> CreateDatabaseAsync(CustomerDatabaseUpsertDto request, CancellationToken cancellationToken = default);
    Task UpdateDatabaseAsync(CustomerDatabaseUpsertDto request, CancellationToken cancellationToken = default);
    Task SyncDatabaseStatusAsync(int databaseId, int serviceStatus, CancellationToken cancellationToken = default);
}
