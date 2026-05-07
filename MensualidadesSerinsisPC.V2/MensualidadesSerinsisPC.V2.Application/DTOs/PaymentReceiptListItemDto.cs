namespace MensualidadesSerinsisPC.V2.Application.DTOs;

public class PaymentReceiptListItemDto
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string PaymentMethodName { get; set; } = string.Empty;
    public DateTime PaidAt { get; set; }
    public decimal ReceivedAmount { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string RegisteredBy { get; set; } = string.Empty;
}
