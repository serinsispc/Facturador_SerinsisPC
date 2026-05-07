using MensualidadesSerinsisPC.V2.Application.DTOs;

namespace MensualidadesSerinsisPC.V2.Application.Abstractions;

public interface IAutomationOperationsService
{
    Task<IReadOnlyList<AutomationLogItemDto>> GetAutomationLogsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<NotificationQueueItemDto>> GetPendingNotificationsAsync(CancellationToken cancellationToken = default);
    Task RunFullDailyAutomationAsync(CancellationToken cancellationToken = default);
    Task MarkNotificationAsSentAsync(int notificationLogId, CancellationToken cancellationToken = default);
    Task MarkNotificationAsErrorAsync(int notificationLogId, CancellationToken cancellationToken = default);
}
