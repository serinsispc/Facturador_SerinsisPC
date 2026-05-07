using MensualidadesSerinsisPC.V2.Application.Abstractions;
using MensualidadesSerinsisPC.V2.Application.DTOs;
using MensualidadesSerinsisPC.V2.Domain.Enums;
using MensualidadesSerinsisPC.V2.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MensualidadesSerinsisPC.V2.Infrastructure.Services;

public class DashboardService(MensualidadesV2DbContext dbContext) : IDashboardService
{
    public async Task<DashboardOverviewDto> GetOverviewAsync(CancellationToken cancellationToken = default)
    {
        DateTime today = DateTime.Today;
        DateTime monthStart = new(today.Year, today.Month, 1);
        DateTime nextMonth = monthStart.AddMonths(1);

        int totalCustomers = await dbContext.Customers.CountAsync(cancellationToken);
        int activeCustomers = await dbContext.Customers.CountAsync(x => x.ServiceStatus == ServiceStatusType.GoodStanding, cancellationToken);
        int warningCustomers = await dbContext.Customers.CountAsync(x => x.ServiceStatus == ServiceStatusType.Warning, cancellationToken);
        int suspendedCustomers = await dbContext.Customers.CountAsync(x => x.ServiceStatus == ServiceStatusType.Suspended, cancellationToken);
        int pendingInvoices = await dbContext.Invoices.CountAsync(x => x.PendingAmount > 0, cancellationToken);
        int pendingNotifications = await dbContext.NotificationLogs.CountAsync(x => x.DeliveryStatus == "PENDING" || x.DeliveryStatus == "ERROR", cancellationToken);
        int scheduledBillingsToday = await dbContext.BillingSchedules.CountAsync(x => x.ScheduledDate == today, cancellationToken);

        decimal pendingPortfolio = await dbContext.Invoices
            .Where(x => x.PendingAmount > 0)
            .Select(x => x.PendingAmount)
            .DefaultIfEmpty(0)
            .SumAsync(cancellationToken);

        decimal currentMonthIncome = await dbContext.PaymentReceipts
            .Where(x => x.PaidAt >= monthStart && x.PaidAt < nextMonth)
            .Select(x => x.ReceivedAmount)
            .DefaultIfEmpty(0)
            .SumAsync(cancellationToken);

        List<DashboardDueItemDto> topPendingCustomers = await (
            from invoice in dbContext.Invoices
            join customer in dbContext.Customers on invoice.CustomerId equals customer.Id
            join subscription in dbContext.CustomerSubscriptions on invoice.CustomerSubscriptionId equals subscription.Id
            join plan in dbContext.ServicePlans on subscription.ServicePlanId equals plan.Id
            where invoice.PendingAmount > 0
            orderby invoice.DueDate, invoice.PendingAmount descending
            select new DashboardDueItemDto
            {
                CustomerName = customer.CommercialName,
                PlanName = plan.Name,
                DueDate = invoice.DueDate,
                PendingAmount = invoice.PendingAmount,
                ServiceStatusName = customer.ServiceStatus.ToString()
            })
            .Take(5)
            .ToListAsync(cancellationToken);

        List<DashboardNotificationItemDto> notificationQueuePreview = await (
            from notification in dbContext.NotificationLogs
            join customer in dbContext.Customers on notification.CustomerId equals customer.Id
            where notification.DeliveryStatus == "PENDING" || notification.DeliveryStatus == "ERROR"
            orderby notification.Id descending
            select new DashboardNotificationItemDto
            {
                CommercialName = customer.CommercialName,
                Channel = notification.Channel,
                DeliveryStatus = notification.DeliveryStatus,
                Recipient = notification.Recipient
            })
            .Take(5)
            .ToListAsync(cancellationToken);

        return new DashboardOverviewDto
        {
            Summary = new DashboardSummaryDto
            {
                ActiveCustomers = activeCustomers,
                WarningCustomers = warningCustomers,
                SuspendedCustomers = suspendedCustomers,
                PendingPortfolio = pendingPortfolio,
                CurrentMonthIncome = currentMonthIncome
            },
            TotalCustomers = totalCustomers,
            PendingInvoices = pendingInvoices,
            PendingNotifications = pendingNotifications,
            ScheduledBillingsToday = scheduledBillingsToday,
            TopPendingCustomers = topPendingCustomers,
            NotificationQueuePreview = notificationQueuePreview
        };
    }
}
