using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Facturador_SerinsisPC.Models.ViewModels
{
    public class V_CobroWhatsApp
    {
        public int id { get; set; }
        public string celular { get; set; }
        public string nombreComercial { get; set; }
        public string nombreRepresentate { get; set; }
        public int sedes { get; set; }
        public decimal valorPlan { get; set; }
        public int mesesEnMora { get; set; }
        public decimal totalA_Pagar { get; set; }
        public string nombrePlan { get; set; }
    }
}