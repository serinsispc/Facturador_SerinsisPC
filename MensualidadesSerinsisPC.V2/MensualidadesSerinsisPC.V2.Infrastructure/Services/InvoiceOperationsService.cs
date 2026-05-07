using MensualidadesSerinsisPC.V2.Application.Abstractions;
using MensualidadesSerinsisPC.V2.Application.DTOs;
using MensualidadesSerinsisPC.V2.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MensualidadesSerinsisPC.V2.Infrastructure.Services;

public class InvoiceOperationsService(MensualidadesV2DbContext dbContext) : IInvoiceOperationsService
{
    public async Task<IReadOnlyList<InvoiceListItemDto>> GetInvoicesAsync(CancellationToken cancellationToken = default)
    {
        return await (
            from invoice in dbContext.Invoices
            join customer in dbContext.Customers on invoice.CustomerId equals customer.Id
            join subscription in dbContext.CustomerSubscriptions on invoice.CustomerSubscriptionId equals subscription.Id
            join plan in dbContext.ServicePlans on subscription.ServicePlanId equals plan.Id
            orderby invoice.IssuedAt descending, invoice.Id descending
            select new InvoiceListItemDto
            {
                Id = invoice.Id,
                CustomerId = invoice.CustomerId,
                CustomerSubscriptionId = invoice.CustomerSubscriptionId,
                CustomerName = customer.CommercialName,
                PlanName = plan.Name,
                IssuedAt = invoice.IssuedAt,
                DueDate = invoice.DueDate,
                PeriodStart = invoice.PeriodStart,
                PeriodEnd = invoice.PeriodEnd,
                TotalAmount = invoice.TotalAmount,
                PendingAmount = invoice.PendingAmount,
                InvoiceStatusName = invoice.Status.ToString()
            }).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<BillingScheduleListItemDto>> GetBillingSchedulesAsync(CancellationToken cancellationToken = default)
    {
        var sql = """
            SELECT
                bs.Id,
                bs.CustomerId,
                bs.CustomerSubscriptionId,
                c.CommercialName AS CustomerName,
                sp.Name AS PlanName,
                bs.ScheduledDate,
                bs.PeriodStart,
                bs.PeriodEnd,
                bs.AmountToBill,
                bs.ScheduleState,
                bs.InvoiceId,
                bs.ProcessedAt,
                bs.LastMessage
            FROM dbo.BillingSchedules bs
            INNER JOIN dbo.Customers c ON c.Id = bs.CustomerId
            INNER JOIN dbo.CustomerSubscriptions cs ON cs.Id = bs.CustomerSubscriptionId
            INNER JOIN dbo.ServicePlans sp ON sp.Id = cs.ServicePlanId
            ORDER BY bs.ScheduledDate DESC, bs.Id DESC
            """;

        return await dbContext.Database.SqlQuery<BillingScheduleListItemDto>($"{sql}")
            .ToListAsync(cancellationToken);
    }

    public Task QueueDueBillingsAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.Database.ExecuteSqlRawAsync("EXEC dbo.sp_QueueDueBillings", cancellationToken);
    }

    public Task GenerateInvoicesDueTodayAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.Database.ExecuteSqlRawAsync("EXEC dbo.sp_GenerateInvoicesDueToday", cancellationToken);
    }

    public async Task RunDailyBillingCycleAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.ExecuteSqlRawAsync("EXEC dbo.sp_QueueDueBillings", cancellationToken);
        await dbContext.Database.ExecuteSqlRawAsync("EXEC dbo.sp_GenerateInvoicesDueToday", cancellationToken);
        await dbContext.Database.ExecuteSqlRawAsync("EXEC dbo.sp_MarkOverdueInvoices", cancellationToken);
    }
}
