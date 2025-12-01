using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Facturador_SerinsisPC.Models.ViewModels
{
    public class EstadoCuentaClientes
    {
        [Required]
        public int id { get; set; }
        [Required]
        public string nombreRepresentate { get; set; }
        [Required]
        public string nombreComercial { get; set; }
        [Required]
        public string celular {  get; set; }
        [Required]
        public decimal total { get; set; }
        [Required]
        public string estado { get; set; }
    }
}