using System.ComponentModel.DataAnnotations;

namespace Facturador_SerinsisPC.Models.ViewModels
{
    public class MetodoPago
    {
        [Required]
        public int id { get; set; }
        [Required]
        public string nombreMetodo { get; set; }
    }
}
