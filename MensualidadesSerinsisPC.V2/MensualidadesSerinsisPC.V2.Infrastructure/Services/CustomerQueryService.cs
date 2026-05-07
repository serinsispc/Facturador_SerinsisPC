using MensualidadesSerinsisPC.V2.Application.Abstractions;
using MensualidadesSerinsisPC.V2.Application.DTOs;
using MensualidadesSerinsisPC.V2.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MensualidadesSerinsisPC.V2.Infrastructure.Services;

public class CustomerQueryService(MensualidadesV2DbContext dbContext) : ICustomerQueryService
{
    public async Task<IReadOnlyList<CustomerListItemDto>> GetCustomersAsync(CancellationToken cancellationToken = default)
    {
        var query =
            from customer in dbContext.Customers
            join subscription in dbContext.CustomerSubscriptions on customer.Id equals subscription.CustomerId into subscriptions
            from subscription in subscriptions.DefaultIfEmpty()
            select new CustomerListItemDto
            {
                Id = customer.Id,
                CommercialName = customer.CommercialName,
                ContactName = customer.ContactName,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                ServiceStatusName = customer.ServiceStatus.ToString(),
                NextBillingDate = subscription != null ? subscription.NextBillingDate : null,
                PendingAmount = dbContext.Invoices
                    .Where(x => x.CustomerId == customer.Id && x.PendingAmount > 0)
                    .Select(x => x.PendingAmount)
                    .DefaultIfEmpty(0)
                    .Sum()
            };

        return await query
            .OrderBy(x => x.CommercialName)
            .ToListAsync(cancellationToken);
    }
}
