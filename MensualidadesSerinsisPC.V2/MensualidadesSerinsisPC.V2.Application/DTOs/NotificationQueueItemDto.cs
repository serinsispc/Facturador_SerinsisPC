namespace MensualidadesSerinsisPC.V2.Application.DTOs;

public class NotificationQueueItemDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CommercialName { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public string Recipient { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string MessageBody { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
    public string DeliveryStatus { get; set; } = string.Empty;
}
