using MensualidadesSerinsisPC.V2.Domain.Enums;

namespace MensualidadesSerinsisPC.V2.Domain.Entities;

public class ServiceStatusHistory
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public ServiceStatusType PreviousStatus { get; set; }
    public ServiceStatusType NewStatus { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}
