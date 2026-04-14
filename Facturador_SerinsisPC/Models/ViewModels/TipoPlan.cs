using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Facturador_SerinsisPC.Models.ViewModels
{
    public class TipoPlan
    {
        [Required]
        public int id { get; set; }
        [Required]
        public string nombrePlan { get; set; }
        public int periodicidadMeses { get; set; }
    }
}
