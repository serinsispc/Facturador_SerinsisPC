namespace MensualidadesSerinsisPC.V2.Domain.Entities;

public class PaymentAllocation
{
    public int Id { get; set; }
    public int PaymentReceiptId { get; set; }
    public int InvoiceId { get; set; }
    public decimal AppliedAmount { get; set; }
}
