using MensualidadesSerinsisPC.V2.Application.Abstractions;
using MensualidadesSerinsisPC.V2.Application.DTOs;
using MensualidadesSerinsisPC.V2.Domain.Entities;
using MensualidadesSerinsisPC.V2.Domain.Enums;
using MensualidadesSerinsisPC.V2.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MensualidadesSerinsisPC.V2.Infrastructure.Services;

public class CustomerDatabaseService(MensualidadesV2DbContext dbContext) : ICustomerDatabaseService
{
    public async Task<IReadOnlyList<CustomerDatabaseListItemDto>> GetDatabasesAsync(CancellationToken cancellationToken = default)
    {
        return await (
            from database in dbContext.CustomerDatabases
            join customer in dbContext.Customers on database.CustomerId equals customer.Id
            orderby customer.CommercialName, database.DatabaseName
            select new CustomerDatabaseListItemDto
            {
                Id = database.Id,
                CustomerId = database.CustomerId,
                CustomerName = customer.CommercialName,
                DatabaseName = database.DatabaseName,
                ServerName = database.ServerName,
                IsOnline = database.IsOnline,
                ServiceStatusName = database.ServiceStatus.ToString(),
                CurrentMessage = database.CurrentMessage,
                LastSynchronizedAt = database.LastSynchronizedAt
            }).ToListAsync(cancellationToken);
    }

    public async Task<CustomerDatabaseUpsertDto?> GetDatabaseForEditAsync(int databaseId, CancellationToken cancellationToken = default)
    {
        return await dbContext.CustomerDatabases
            .Where(x => x.Id == databaseId)
            .Select(x => new CustomerDatabaseUpsertDto
            {
                Id = x.Id,
                CustomerId = x.CustomerId,
                DatabaseName = x.DatabaseName,
                ServerName = x.ServerName,
                IsOnline = x.IsOnline,
                CurrentMessage = x.CurrentMessage
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> CreateDatabaseAsync(CustomerDatabaseUpsertDto request, CancellationToken cancellationToken = default)
    {
        await ValidateCustomerAsync(request.CustomerId, cancellationToken);

        string normalizedName = Normalize(request.DatabaseName);
        bool exists = await dbContext.CustomerDatabases.AnyAsync(x => x.DatabaseName == normalizedName, cancellationToken);
        if (exists)
        {
            throw new InvalidOperationException("Ya existe una base registrada con ese nombre.");
        }

        var entity = new CustomerDatabase
        {
            CustomerId = request.CustomerId,
            DatabaseName = normalizedName,
            ServerName = Normalize(request.ServerName),
            IsOnline = request.IsOnline,
            CurrentMessage = NormalizeMessage(request.CurrentMessage),
            ServiceStatus = request.IsOnline ? ServiceStatusType.GoodStanding : ServiceStatusType.Suspended,
            LastSynchronizedAt = DateTime.UtcNow
        };

        dbContext.CustomerDatabases.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task UpdateDatabaseAsync(CustomerDatabaseUpsertDto request, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.CustomerDatabases.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (entity is null)
        {
            throw new InvalidOperationException("No se encontro la base de datos que intentas editar.");
        }

        await ValidateCustomerAsync(request.CustomerId, cancellationToken);

        string normalizedName = Normalize(request.DatabaseName);
        bool exists = await dbContext.CustomerDatabases.AnyAsync(
            x => x.Id != request.Id && x.DatabaseName == normalizedName,
            cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException("Otra base ya usa ese mismo nombre.");
        }

        entity.CustomerId = request.CustomerId;
        entity.DatabaseName = normalizedName;
        entity.ServerName = Normalize(request.ServerName);
        entity.IsOnline = request.IsOnline;
        entity.CurrentMessage = NormalizeMessage(request.CurrentMessage);
        entity.ServiceStatus = request.IsOnline ? ServiceStatusType.GoodStanding : ServiceStatusType.Suspended;
        entity.LastSynchronizedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SyncDatabaseStatusAsync(int databaseId, int serviceStatus, CancellationToken cancellationToken = default)
    {
        if (!Enum.IsDefined(typeof(ServiceStatusType), serviceStatus))
        {
            throw new InvalidOperationException("El estado solicitado para la base no es valido.");
        }

        var entity = await dbContext.CustomerDatabases.FirstOrDefaultAsync(x => x.Id == databaseId, cancellationToken);
        if (entity is null)
        {
            throw new InvalidOperationException("No se encontro la base de datos para sincronizar.");
        }

        entity.ServiceStatus = (ServiceStatusType)serviceStatus;
        entity.IsOnline = serviceStatus != (int)ServiceStatusType.Suspended;
        entity.CurrentMessage = serviceStatus switch
        {
            0 => "SERVICIO ACTIVO Y AL DIA.",
            1 => "AVISO DE PAGO PENDIENTE. EVITE LA SUSPENSION.",
            2 => "SERVICIO SUSPENDIDO POR FALTA DE PAGO.",
            _ => entity.CurrentMessage
        };
        entity.LastSynchronizedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task ValidateCustomerAsync(int customerId, CancellationToken cancellationToken)
    {
        bool exists = await dbContext.Customers.AnyAsync(x => x.Id == customerId, cancellationToken);
        if (!exists)
        {
            throw new InvalidOperationException("El cliente seleccionado no existe.");
        }
    }

    private static string Normalize(string? value)
    {
        return (value ?? string.Empty).Trim();
    }

    private static string NormalizeMessage(string? value)
    {
        string normalized = Normalize(value);
        return string.IsNullOrWhiteSpace(normalized) ? "SERVICIO ACTIVO Y AL DIA." : normalized;
    }
}
