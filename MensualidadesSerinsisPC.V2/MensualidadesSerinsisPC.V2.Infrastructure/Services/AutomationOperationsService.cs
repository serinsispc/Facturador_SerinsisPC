using MensualidadesSerinsisPC.V2.Application.Abstractions;
using MensualidadesSerinsisPC.V2.Application.DTOs;
using MensualidadesSerinsisPC.V2.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MensualidadesSerinsisPC.V2.Infrastructure.Services;

public class AutomationOperationsService(MensualidadesV2DbContext dbContext) : IAutomationOperationsService
{
    public async Task<IReadOnlyList<AutomationLogItemDto>> GetAutomationLogsAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.AutomationProcessLogs
            .OrderByDescending(x => x.ExecutedAt)
            .ThenByDescending(x => x.Id)
            .Select(x => new AutomationLogItemDto
            {
                Id = x.Id,
                ProcessName = x.ProcessName,
                ExecutedAt = x.ExecutedAt,
                ProcessedCount = x.ProcessedCount,
                SuccessfulCount = x.SuccessfulCount,
                ErrorCount = x.ErrorCount,
                Details = x.Details
            })
            .Take(100)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<NotificationQueueItemDto>> GetPendingNotificationsAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                nl.Id,
                nl.CustomerId,
                c.CommercialName,
                c.ContactName,
                c.PhoneNumber,
                c.Email,
                nl.Channel,
                nl.Recipient,
                nl.Subject,
                nl.MessageBody,
                nl.SentAt,
                nl.DeliveryStatus
            FROM dbo.NotificationLogs nl
            INNER JOIN dbo.Customers c ON c.Id = nl.CustomerId
            WHERE nl.DeliveryStatus IN ('PENDING', 'ERROR')
            ORDER BY nl.Id DESC
            """;

        return await dbContext.Database.SqlQuery<NotificationQueueItemDto>($"{sql}")
            .ToListAsync(cancellationToken);
    }

    public async Task RunFullDailyAutomationAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.ExecuteSqlRawAsync("EXEC dbo.sp_RunFullDailyAutomation", cancellationToken);
    }

    public async Task MarkNotificationAsSentAsync(int notificationLogId, CancellationToken cancellationToken = default)
    {
        var parameters = new[] { new SqlParameter("@NotificationLogId", notificationLogId) };
        await dbContext.Database.ExecuteSqlRawAsync("EXEC dbo.sp_MarkNotificationAsSent @NotificationLogId", parameters, cancellationToken);
    }

    public async Task MarkNotificationAsErrorAsync(int notificationLogId, CancellationToken cancellationToken = default)
    {
        var parameters = new[] { new SqlParameter("@NotificationLogId", notificationLogId) };
        await dbContext.Database.ExecuteSqlRawAsync("EXEC dbo.sp_MarkNotificationAsError @NotificationLogId", parameters, cancellationToken);
    }
}
