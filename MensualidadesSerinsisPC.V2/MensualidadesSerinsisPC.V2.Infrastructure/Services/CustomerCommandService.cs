using MensualidadesSerinsisPC.V2.Application.Abstractions;
using MensualidadesSerinsisPC.V2.Application.DTOs;
using MensualidadesSerinsisPC.V2.Domain.Entities;
using MensualidadesSerinsisPC.V2.Domain.Enums;
using MensualidadesSerinsisPC.V2.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MensualidadesSerinsisPC.V2.Infrastructure.Services;

public class CustomerCommandService(MensualidadesV2DbContext dbContext) : ICustomerCommandService
{
    public async Task<CustomerUpsertDto?> GetCustomerForEditAsync(int customerId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Customers
            .Where(x => x.Id == customerId)
            .Select(x => new CustomerUpsertDto
            {
                Id = x.Id,
                DocumentNumber = x.DocumentNumber,
                BusinessName = x.BusinessName,
                CommercialName = x.CommercialName,
                ContactName = x.ContactName,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                Address = x.Address,
                Notes = x.Notes,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> CreateCustomerAsync(CustomerUpsertDto request, CancellationToken cancellationToken = default)
    {
        string normalizedDocument = Normalize(request.DocumentNumber);
        string normalizedCommercialName = Normalize(request.CommercialName);

        bool exists = await dbContext.Customers.AnyAsync(
            x => x.DocumentNumber == normalizedDocument || x.CommercialName == normalizedCommercialName,
            cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException("Ya existe un cliente con el mismo documento o nombre comercial.");
        }

        var entity = new Customer
        {
            DocumentNumber = normalizedDocument,
            BusinessName = Normalize(request.BusinessName),
            CommercialName = normalizedCommercialName,
            ContactName = Normalize(request.ContactName),
            PhoneNumber = Normalize(request.PhoneNumber),
            Email = Normalize(request.Email),
            Address = Normalize(request.Address),
            Notes = Normalize(request.Notes),
            IsActive = request.IsActive,
            ServiceStatus = ServiceStatusType.GoodStanding,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Customers.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task UpdateCustomerAsync(CustomerUpsertDto request, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.Customers.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity is null)
        {
            throw new InvalidOperationException("No se encontro el cliente que intentas editar.");
        }

        string normalizedDocument = Normalize(request.DocumentNumber);
        string normalizedCommercialName = Normalize(request.CommercialName);

        bool exists = await dbContext.Customers.AnyAsync(
            x => x.Id != request.Id && (x.DocumentNumber == normalizedDocument || x.CommercialName == normalizedCommercialName),
            cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException("Otro cliente ya usa ese documento o nombre comercial.");
        }

        entity.DocumentNumber = normalizedDocument;
        entity.BusinessName = Normalize(request.BusinessName);
        entity.CommercialName = normalizedCommercialName;
        entity.ContactName = Normalize(request.ContactName);
        entity.PhoneNumber = Normalize(request.PhoneNumber);
        entity.Email = Normalize(request.Email);
        entity.Address = Normalize(request.Address);
        entity.Notes = Normalize(request.Notes);
        entity.IsActive = request.IsActive;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateServiceStatusAsync(int customerId, int serviceStatus, CancellationToken cancellationToken = default)
    {
        if (!Enum.IsDefined(typeof(ServiceStatusType), serviceStatus))
        {
            throw new InvalidOperationException("El estado de servicio solicitado no es valido.");
        }

        var entity = await dbContext.Customers.FirstOrDefaultAsync(x => x.Id == customerId, cancellationToken);
        if (entity is null)
        {
            throw new InvalidOperationException("No se encontro el cliente para actualizar su estado.");
        }

        entity.ServiceStatus = (ServiceStatusType)serviceStatus;
        entity.IsActive = serviceStatus != (int)ServiceStatusType.Suspended;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static string Normalize(string? value)
    {
        return (value ?? string.Empty).Trim();
    }
}
