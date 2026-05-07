using MensualidadesSerinsisPC.V2.Application.Abstractions;
using MensualidadesSerinsisPC.V2.Application.DTOs;
using MensualidadesSerinsisPC.V2.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MensualidadesSerinsisPC.V2.Infrastructure.Services;

public class PaymentOperationsService(MensualidadesV2DbContext dbContext) : IPaymentOperationsService
{
    public async Task<IReadOnlyList<LookupOptionDto>> GetPaymentMethodOptionsAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.PaymentMethods
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .Select(x => new LookupOptionDto
            {
                Id = x.Id,
                Label = x.Name
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<InvoiceListItemDto>> GetPendingInvoicesAsync(CancellationToken cancellationToken = default)
    {
        return await (
            from invoice in dbContext.Invoices
            join customer in dbContext.Customers on invoice.CustomerId equals customer.Id
            join subscription in dbContext.CustomerSubscriptions on invoice.CustomerSubscriptionId equals subscription.Id
            join plan in dbContext.ServicePlans on subscription.ServicePlanId equals plan.Id
            where invoice.PendingAmount > 0
            orderby invoice.DueDate, invoice.Id
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

    public async Task<IReadOnlyList<PaymentReceiptListItemDto>> GetRecentPaymentsAsync(CancellationToken cancellationToken = default)
    {
        return await (
            from payment in dbContext.PaymentReceipts
            join customer in dbContext.Customers on payment.CustomerId equals customer.Id
            join method in dbContext.PaymentMethods on payment.PaymentMethodId equals method.Id
            orderby payment.PaidAt descending, payment.Id descending
            select new PaymentReceiptListItemDto
            {
                Id = payment.Id,
                CustomerName = customer.CommercialName,
                PaymentMethodName = method.Name,
                PaidAt = payment.PaidAt,
                ReceivedAmount = payment.ReceivedAmount,
                ReceiptNumber = payment.ReceiptNumber,
                Reference = payment.Reference,
                RegisteredBy = payment.RegisteredBy
            }).Take(100).ToListAsync(cancellationToken);
    }

    public async Task RegisterPaymentAsync(PaymentRegisterDto request, string registeredBy, CancellationToken cancellationToken = default)
    {
        var parameters = new[]
        {
            new SqlParameter("@InvoiceId", request.InvoiceId),
            new SqlParameter("@PaymentMethodId", request.PaymentMethodId),
            new SqlParameter("@PaidAt", request.PaidAt),
            new SqlParameter("@ReceivedAmount", request.ReceivedAmount),
            new SqlParameter("@ReceiptNumber", request.ReceiptNumber ?? string.Empty),
            new SqlParameter("@Reference", request.Reference ?? string.Empty),
            new SqlParameter("@Notes", request.Notes ?? string.Empty),
            new SqlParameter("@RegisteredBy", registeredBy)
        };

        await dbContext.Database.ExecuteSqlRawAsync(
            "EXEC dbo.sp_RegisterPayment @InvoiceId, @PaymentMethodId, @PaidAt, @ReceivedAmount, @ReceiptNumber, @Reference, @Notes, @RegisteredBy",
            parameters,
            cancellationToken);
    }
}
