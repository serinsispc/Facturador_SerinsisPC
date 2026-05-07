namespace MensualidadesSerinsisPC.V2.Domain.Entities;

public class NotificationLog
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Channel { get; set; } = string.Empty;
    public string Recipient { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string MessageBody { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public string DeliveryStatus { get; set; } = string.Empty;
}
