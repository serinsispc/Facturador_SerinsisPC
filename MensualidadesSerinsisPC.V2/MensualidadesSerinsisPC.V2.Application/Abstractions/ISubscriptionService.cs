using MensualidadesSerinsisPC.V2.Application.DTOs;

namespace MensualidadesSerinsisPC.V2.Application.Abstractions;

public interface ISubscriptionService
{
    Task<IReadOnlyList<SubscriptionListItemDto>> GetSubscriptionsAsync(CancellationToken cancellationToken = default);
    Task<SubscriptionUpsertDto?> GetSubscriptionForEditAsync(int subscriptionId, CancellationToken cancellationToken = default);
    Task<int> CreateSubscriptionAsync(SubscriptionUpsertDto request, CancellationToken cancellationToken = default);
    Task UpdateSubscriptionAsync(SubscriptionUpsertDto request, CancellationToken cancellationToken = default);
}
