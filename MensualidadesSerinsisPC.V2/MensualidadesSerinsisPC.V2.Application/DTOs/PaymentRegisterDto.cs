using System.ComponentModel.DataAnnotations;

namespace MensualidadesSerinsisPC.V2.Application.DTOs;

public class PaymentRegisterDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona una factura.")]
    public int InvoiceId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un metodo de pago.")]
    public int PaymentMethodId { get; set; }

    [Required(ErrorMessage = "La fecha del pago es obligatoria.")]
    public DateTime PaidAt { get; set; } = DateTime.Now;

    [Range(typeof(decimal), "1", "999999999", ErrorMessage = "El valor recibido debe ser mayor a cero.")]
    public decimal ReceivedAmount { get; set; }

    [StringLength(100, ErrorMessage = "El comprobante admite maximo 100 caracteres.")]
    public string ReceiptNumber { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "La referencia admite maximo 100 caracteres.")]
    public string Reference { get; set; } = string.Empty;

    [StringLength(250, ErrorMessage = "La observacion admite maximo 250 caracteres.")]
    public string Notes { get; set; } = string.Empty;
}
