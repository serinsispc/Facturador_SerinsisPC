using System.ComponentModel.DataAnnotations;

namespace MensualidadesSerinsisPC.V2.Web.Models;

public class AdminLoginDto
{
    [Required(ErrorMessage = "Ingresa el usuario.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa la clave.")]
    public string Password { get; set; } = string.Empty;
}
