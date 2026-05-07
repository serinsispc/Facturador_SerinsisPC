using MensualidadesSerinsisPC.V2.Application.Abstractions;
using MensualidadesSerinsisPC.V2.Application.DTOs;
using MensualidadesSerinsisPC.V2.Domain.Entities;
using MensualidadesSerinsisPC.V2.Domain.Enums;
using MensualidadesSerinsisPC.V2.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MensualidadesSerinsisPC.V2.Infrastructure.Services;

public class SubscriptionService(MensualidadesV2DbContext dbContext) : ISubscriptionService
{
    public async Task<IReadOnlyList<SubscriptionListItemDto>> GetSubscriptionsAsync(CancellationToken cancellationToken = default)
    {
        return await (
            from subscription in dbContext.CustomerSubscriptions
            join customer in dbContext.Customers on subscription.CustomerId equals customer.Id
            join plan in dbContext.ServicePlans on subscription.ServicePlanId equals plan.Id
            orderby customer.CommercialName, subscription.Id descending
            select new SubscriptionListItemDto
            {
                Id = subscription.Id,
                CustomerId = subscription.CustomerId,
                CustomerName = customer.CommercialName,
                ServicePlanId = subscription.ServicePlanId,
                PlanName = plan.Name,
                BillingPeriodMonths = (int)plan.BillingPeriod,
                StartDate = subscription.StartDate,
                PaymentDay = subscription.PaymentDay,
                NextBillingDate = subscription.NextBillingDate,
                LastPaymentDate = subscription.LastPaymentDate,
                GraceDays = subscription.GraceDays,
                AutomaticBillingEnabled = subscription.AutomaticBillingEnabled,
                AutomaticCollectionEnabled = subscription.AutomaticCollectionEnabled,
                ServiceStatusName = subscription.ServiceStatus.ToString()
            }).ToListAsync(cancellationToken);
    }

    public async Task<SubscriptionUpsertDto?> GetSubscriptionForEditAsync(int subscriptionId, CancellationToken cancellationToken = default)
    {
        return await dbContext.CustomerSubscriptions
            .Where(x => x.Id == subscriptionId)
            .Select(x => new SubscriptionUpsertDto
            {
                Id = x.Id,
                CustomerId = x.CustomerId,
                ServicePlanId = x.ServicePlanId,
                StartDate = x.StartDate,
                PaymentDay = x.PaymentDay,
                NextBillingDate = x.NextBillingDate,
                LastPaymentDate = x.LastPaymentDate,
                GraceDays = x.GraceDays,
                AutomaticBillingEnabled = x.AutomaticBillingEnabled,
                AutomaticCollectionEnabled = x.AutomaticCollectionEnabled
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> CreateSubscriptionAsync(SubscriptionUpsertDto request, CancellationToken cancellationToken = default)
    {
        await ValidateForeignKeysAsync(request.CustomerId, request.ServicePlanId, cancellationToken);

        bool exists = await dbContext.CustomerSubscriptions.AnyAsync(
            x => x.CustomerId == request.CustomerId && x.ServicePlanId == request.ServicePlanId,
            cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException("Ese cliente ya tiene una suscripcion creada para el plan seleccionado.");
        }

        var entity = new CustomerSubscription
        {
            CustomerId = request.CustomerId,
            ServicePlanId = request.ServicePlanId,
            StartDate = request.StartDate.Date,
            PaymentDay = request.PaymentDay,
            NextBillingDate = request.NextBillingDate.Date,
            LastPaymentDate = request.LastPaymentDate?.Date,
            GraceDays = request.GraceDays,
            AutomaticBillingEnabled = request.AutomaticBillingEnabled,
            AutomaticCollectionEnabled = request.AutomaticCollectionEnabled,
            ServiceStatus = ServiceStatusType.GoodStanding
        };

        dbContext.CustomerSubscriptions.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task UpdateSubscriptionAsync(SubscriptionUpsertDto request, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.CustomerSubscriptions.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity is null)
        {
            throw new InvalidOperationException("No se encontro la suscripcion que intentas editar.");
        }

        await ValidateForeignKeysAsync(request.CustomerId, request.ServicePlanId, cancellationToken);

        bool exists = await dbContext.CustomerSubscriptions.AnyAsync(
            x => x.Id != request.Id && x.CustomerId == request.CustomerId && x.ServicePlanId == request.ServicePlanId,
            cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException("Ya existe otra suscripcion para ese mismo cliente y plan.");
        }

        entity.CustomerId = request.CustomerId;
        entity.ServicePlanId = request.ServicePlanId;
        entity.StartDate = request.StartDate.Date;
        entity.PaymentDay = request.PaymentDay;
        entity.NextBillingDate = request.NextBillingDate.Date;
        entity.LastPaymentDate = request.LastPaymentDate?.Date;
        entity.GraceDays = request.GraceDays;
        entity.AutomaticBillingEnabled = request.AutomaticBillingEnabled;
        entity.AutomaticCollectionEnabled = request.AutomaticCollectionEnabled;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task ValidateForeignKeysAsync(int customerId, int servicePlanId, CancellationToken cancellationToken)
    {
        bool customerExists = await dbContext.Customers.AnyAsync(x => x.Id == customerId, cancellationToken);
        if (!customerExists)
        {
            throw new InvalidOperationException("El cliente seleccionado no existe.");
        }

        bool planExists = await dbContext.ServicePlans.AnyAsync(x => x.Id == servicePlanId && x.IsActive, cancellationToken);
        if (!planExists)
        {
            throw new InvalidOperationException("El plan seleccionado no existe o esta inactivo.");
        }
    }
}
