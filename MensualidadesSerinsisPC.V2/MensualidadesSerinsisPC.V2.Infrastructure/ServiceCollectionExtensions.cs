using MensualidadesSerinsisPC.V2.Application.Abstractions;
using MensualidadesSerinsisPC.V2.Application.Services;
using MensualidadesSerinsisPC.V2.Infrastructure.Persistence;
using MensualidadesSerinsisPC.V2.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MensualidadesSerinsisPC.V2.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMensualidadesV2Infrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("MensualidadesV2")
            ?? "Server=.;Database=DBMensualidadesSerinsisPC_V2;Trusted_Connection=True;TrustServerCertificate=True;";

        services.AddDbContext<MensualidadesV2DbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<SolutionBlueprintService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<ICustomerQueryService, CustomerQueryService>();
        services.AddScoped<ICustomerCommandService, CustomerCommandService>();
        services.AddScoped<ILookupCatalogService, LookupCatalogService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<ICustomerDatabaseService, CustomerDatabaseService>();
        services.AddScoped<IInvoiceOperationsService, InvoiceOperationsService>();
        services.AddScoped<IPaymentOperationsService, PaymentOperationsService>();
        services.AddScoped<IIncomeReportingService, IncomeReportingService>();
        services.AddScoped<IAutomationOperationsService, AutomationOperationsService>();

        return services;
    }
}
