using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Facturador_SerinsisPC.Models.ViewModels
{
    public class Facturas
    {
        [Required]
        public int id { get; set; }
        [Required]
        public DateTime fechaFactura { get; set; }
        [Required]
        public int idCliente { get; set; }
        [Required]
        public int idMes { get; set; }
        [Required]
        public decimal valorPlan { get; set; }
        [Required]
        public int sedes { get; set; }
        [Required]
        public decimal valorAPagar { get; set; }
        [Required]
        public int idEstado { get; set; }
        [Required]
        public int contador { get; set; }
        [Required]
        public int yearFactura { get; set; }
    }
}