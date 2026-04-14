using System;
using System.ComponentModel.DataAnnotations;

namespace Facturador_SerinsisPC.Models.ViewModels
{
    public class PagosRecibidos
    {
        [Required]
        public int id { get; set; }
        [Required]
        public DateTime fechaPago { get; set; }
        [Required]
        public int idCliente { get; set; }
        [Required]
        public int idMetodoPago { get; set; }
        [Required]
        public decimal valorRecibido { get; set; }
        public string numeroComprobante { get; set; }
        public string referenciaPago { get; set; }
        public string observacion { get; set; }
        public string usuarioRegistro { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
}
