using MensualidadesSerinsisPC.V2.Domain.Enums;

namespace MensualidadesSerinsisPC.V2.Domain.Entities;

public class CustomerDatabase
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string DatabaseName { get; set; } = string.Empty;
    public string ServerName { get; set; } = string.Empty;
    public bool IsOnline { get; set; } = true;
    public ServiceStatusType ServiceStatus { get; set; } = ServiceStatusType.GoodStanding;
    public string CurrentMessage { get; set; } = string.Empty;
    public DateTime LastSynchronizedAt { get; set; } = DateTime.UtcNow;

    public Customer? Customer { get; set; }
}
