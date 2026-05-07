namespace MensualidadesSerinsisPC.V2.Application.DTOs;

public class CustomerDatabaseListItemDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string ServerName { get; set; } = string.Empty;
    public bool IsOnline { get; set; }
    public string ServiceStatusName { get; set; } = string.Empty;
    public string CurrentMessage { get; set; } = string.Empty;
    public DateTime LastSynchronizedAt { get; set; }
}
