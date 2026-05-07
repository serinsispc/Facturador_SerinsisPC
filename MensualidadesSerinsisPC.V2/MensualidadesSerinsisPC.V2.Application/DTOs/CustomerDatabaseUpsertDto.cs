using System.ComponentModel.DataAnnotations;

namespace MensualidadesSerinsisPC.V2.Application.DTOs;

public class CustomerDatabaseUpsertDto
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un cliente.")]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "El nombre de la base es obligatorio.")]
    [StringLength(80, ErrorMessage = "La base admite maximo 80 caracteres.")]
    public string DatabaseName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El servidor es obligatorio.")]
    [StringLength(120, ErrorMessage = "El servidor admite maximo 120 caracteres.")]
    public string ServerName { get; set; } = string.Empty;

    public bool IsOnline { get; set; } = true;

    [StringLength(500, ErrorMessage = "El mensaje admite maximo 500 caracteres.")]
    public string CurrentMessage { get; set; } = "SERVICIO ACTIVO Y AL DIA.";
}
