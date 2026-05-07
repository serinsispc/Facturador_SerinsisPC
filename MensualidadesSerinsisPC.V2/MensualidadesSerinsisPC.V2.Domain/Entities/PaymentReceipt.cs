namespace MensualidadesSerinsisPC.V2.Domain.Entities;

public class PaymentReceipt
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int PaymentMethodId { get; set; }
    public DateTime PaidAt { get; set; }
    public decimal ReceivedAmount { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public string RegisteredBy { get; set; } = string.Empty;
}
