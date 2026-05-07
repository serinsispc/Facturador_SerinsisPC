using System.ComponentModel.DataAnnotations;

namespace MensualidadesSerinsisPC.V2.Application.DTOs;

public class CustomerUpsertDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El documento es obligatorio.")]
    [StringLength(20, ErrorMessage = "El documento admite maximo 20 caracteres.")]
    public string DocumentNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "La razon social es obligatoria.")]
    [StringLength(150, ErrorMessage = "La razon social admite maximo 150 caracteres.")]
    public string BusinessName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre comercial es obligatorio.")]
    [StringLength(150, ErrorMessage = "El nombre comercial admite maximo 150 caracteres.")]
    public string CommercialName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El contacto principal es obligatorio.")]
    [StringLength(150, ErrorMessage = "El contacto admite maximo 150 caracteres.")]
    public string ContactName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El telefono es obligatorio.")]
    [StringLength(20, ErrorMessage = "El telefono admite maximo 20 caracteres.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingresa un correo valido.")]
    [StringLength(150, ErrorMessage = "El correo admite maximo 150 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "La direccion admite maximo 200 caracteres.")]
    public string Address { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Las notas admiten maximo 500 caracteres.")]
    public string Notes { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
