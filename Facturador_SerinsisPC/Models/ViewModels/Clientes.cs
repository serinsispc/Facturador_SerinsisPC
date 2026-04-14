using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI.WebControls;

namespace Facturador_SerinsisPC.Models.ViewModels
{
    public class Clientes
    {
        [Required]
        public int id { get; set; }
        public int idTipoPlan { get; set; }
        [Required]
        public string nombreComercial { get; set; }
        [Required]
        public string nombreRepresentate { get; set; }
        [Required]
        public string celular { get; set; }
        [Required]
        public string correo { get; set; }
        [Required]
        public int sedes { get; set; }
        [Required]
        public decimal valorPlan { get; set; }
        [Required]
        public string nit { get; set; }
        [Required]
        public int estado { get; set; }
        public int? diaPago { get; set; }
        public DateTime? fechaInicioPlan { get; set; }
        public DateTime? fechaUltimoPago { get; set; }
        public DateTime? fechaProximoPago { get; set; }
        public string observacionCartera { get; set; }
    }
}
