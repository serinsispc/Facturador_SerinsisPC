using MensualidadesSerinsisPC.V2.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MensualidadesSerinsisPC.V2.Infrastructure.Persistence;

public class MensualidadesV2DbContext(DbContextOptions<MensualidadesV2DbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<ServicePlan> ServicePlans => Set<ServicePlan>();
    public DbSet<CustomerSubscription> CustomerSubscriptions => Set<CustomerSubscription>();
    public DbSet<CustomerDatabase> CustomerDatabases => Set<CustomerDatabase>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
    public DbSet<PaymentReceipt> PaymentReceipts => Set<PaymentReceipt>();
    public DbSet<PaymentAllocation> PaymentAllocations => Set<PaymentAllocation>();
    public DbSet<BillingSchedule> BillingSchedules => Set<BillingSchedule>();
    public DbSet<NotificationTemplate> NotificationTemplates => Set<NotificationTemplate>();
    public DbSet<NotificationLog> NotificationLogs => Set<NotificationLog>();
    public DbSet<ServiceStatusHistory> ServiceStatusHistories => Set<ServiceStatusHistory>();
    public DbSet<AutomationProcessLog> AutomationProcessLogs => Set<AutomationProcessLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().ToTable("Customers");
        modelBuilder.Entity<ServicePlan>().ToTable("ServicePlans");
        modelBuilder.Entity<CustomerSubscription>().ToTable("CustomerSubscriptions");
        modelBuilder.Entity<CustomerDatabase>().ToTable("CustomerDatabases");
        modelBuilder.Entity<Invoice>().ToTable("Invoices");
        modelBuilder.Entity<PaymentMethod>().ToTable("PaymentMethods");
        modelBuilder.Entity<PaymentReceipt>().ToTable("PaymentReceipts");
        modelBuilder.Entity<PaymentAllocation>().ToTable("PaymentAllocations");
        modelBuilder.Entity<BillingSchedule>().ToTable("BillingSchedules");
        modelBuilder.Entity<NotificationTemplate>().ToTable("NotificationTemplates");
        modelBuilder.Entity<NotificationLog>().ToTable("NotificationLogs");
        modelBuilder.Entity<ServiceStatusHistory>().ToTable("ServiceStatusHistories");
        modelBuilder.Entity<AutomationProcessLog>().ToTable("AutomationProcessLogs");

        modelBuilder.Entity<Customer>().Property(x => x.DocumentNumber).HasMaxLength(20).IsRequired();
        modelBuilder.Entity<Customer>().Property(x => x.BusinessName).HasMaxLength(150).IsRequired();
        modelBuilder.Entity<Customer>().Property(x => x.CommercialName).HasMaxLength(150).IsRequired();
        modelBuilder.Entity<Customer>().Property(x => x.ContactName).HasMaxLength(150).IsRequired();
        modelBuilder.Entity<Customer>().Property(x => x.PhoneNumber).HasMaxLength(20).IsRequired();
        modelBuilder.Entity<Customer>().Property(x => x.Email).HasMaxLength(150).IsRequired();
        modelBuilder.Entity<Customer>().Property(x => x.Address).HasMaxLength(200);
        modelBuilder.Entity<Customer>().Property(x => x.Notes).HasMaxLength(500);
        modelBuilder.Entity<Customer>().Property(x => x.ServiceStatus).HasColumnName("ServiceStatusId");

        modelBuilder.Entity<ServicePlan>().Property(x => x.Name).HasMaxLength(80).IsRequired();
        modelBuilder.Entity<ServicePlan>().Property(x => x.BasePrice).HasPrecision(18, 2);
        modelBuilder.Entity<ServicePlan>().Property(x => x.BillingPeriod).HasColumnName("BillingPeriodMonths");

        modelBuilder.Entity<CustomerSubscription>().Property(x => x.ServiceStatus).HasColumnName("ServiceStatusId");

        modelBuilder.Entity<CustomerDatabase>().Property(x => x.DatabaseName).HasMaxLength(80).IsRequired();
        modelBuilder.Entity<CustomerDatabase>().Property(x => x.ServerName).HasMaxLength(120).IsRequired();
        modelBuilder.Entity<CustomerDatabase>().Property(x => x.CurrentMessage).HasMaxLength(500);
        modelBuilder.Entity<CustomerDatabase>().Property(x => x.ServiceStatus).HasColumnName("ServiceStatusId");

        modelBuilder.Entity<Invoice>().Property(x => x.TotalAmount).HasPrecision(18, 2);
        modelBuilder.Entity<Invoice>().Property(x => x.PendingAmount).HasPrecision(18, 2);
        modelBuilder.Entity<Invoice>().Property(x => x.Status).HasColumnName("InvoiceStatusId");

        modelBuilder.Entity<PaymentReceipt>().Property(x => x.ReceivedAmount).HasPrecision(18, 2);
        modelBuilder.Entity<PaymentReceipt>().Property(x => x.ReceiptNumber).HasMaxLength(100);
        modelBuilder.Entity<PaymentReceipt>().Property(x => x.Reference).HasMaxLength(100);
        modelBuilder.Entity<PaymentReceipt>().Property(x => x.Notes).HasMaxLength(250);
        modelBuilder.Entity<PaymentReceipt>().Property(x => x.RegisteredBy).HasMaxLength(100);

        modelBuilder.Entity<PaymentAllocation>().Property(x => x.AppliedAmount).HasPrecision(18, 2);
        modelBuilder.Entity<BillingSchedule>().Property(x => x.AmountToBill).HasPrecision(18, 2);
        modelBuilder.Entity<BillingSchedule>().Property(x => x.ScheduleState).HasMaxLength(40).IsRequired();
        modelBuilder.Entity<BillingSchedule>().Property(x => x.LastMessage).HasMaxLength(250);
        modelBuilder.Entity<NotificationTemplate>().Property(x => x.Name).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<NotificationTemplate>().Property(x => x.Channel).HasMaxLength(30).IsRequired();
        modelBuilder.Entity<NotificationTemplate>().Property(x => x.Subject).HasMaxLength(150);
        modelBuilder.Entity<NotificationLog>().Property(x => x.Channel).HasMaxLength(30).IsRequired();
        modelBuilder.Entity<NotificationLog>().Property(x => x.Recipient).HasMaxLength(150).IsRequired();
        modelBuilder.Entity<NotificationLog>().Property(x => x.Subject).HasMaxLength(150);
        modelBuilder.Entity<NotificationLog>().Property(x => x.DeliveryStatus).HasMaxLength(40).IsRequired();
        modelBuilder.Entity<ServiceStatusHistory>().Property(x => x.Reason).HasMaxLength(250).IsRequired();
        modelBuilder.Entity<AutomationProcessLog>().Property(x => x.ProcessName).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<AutomationProcessLog>().Property(x => x.Details).HasMaxLength(500);
    }
}
