using MensualidadesSerinsisPC.V2.Application.DTOs;

namespace MensualidadesSerinsisPC.V2.Application.Abstractions;

public interface IInvoiceOperationsService
{
    Task<IReadOnlyList<InvoiceListItemDto>> GetInvoicesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BillingScheduleListItemDto>> GetBillingSchedulesAsync(CancellationToken cancellationToken = default);
    Task QueueDueBillingsAsync(CancellationToken cancellationToken = default);
    Task GenerateInvoicesDueTodayAsync(CancellationToken cancellationToken = default);
    Task RunDailyBillingCycleAsync(CancellationToken cancellationToken = default);
}
