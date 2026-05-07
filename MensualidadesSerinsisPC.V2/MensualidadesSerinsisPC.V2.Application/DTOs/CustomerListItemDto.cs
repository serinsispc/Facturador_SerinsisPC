namespace MensualidadesSerinsisPC.V2.Application.DTOs;

public class CustomerListItemDto
{
    public int Id { get; set; }
    public string CommercialName { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ServiceStatusName { get; set; } = string.Empty;
    public DateTime? NextBillingDate { get; set; }
    public decimal PendingAmount { get; set; }
}
