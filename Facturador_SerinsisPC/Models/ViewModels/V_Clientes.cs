using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Facturador_SerinsisPC.Models.ViewModels
{
    public class V_Clientes
    {
        public int id {  get; set; }
        public int idTipoPlan { get; set; }
        public string nombrePlan { get; set; }
        public string nit { get; set; }
        public string nombreComercial { get; set; }
        public string nombreRepresentate { get; set; }
        public string celular { get; set; }
        public string correo { get; set; }
        public int sedes { get; set; }
        public decimal valorPlan { get; set; }
        public int estado { get; set; }
        public int? diaPago { get; set; }
        public DateTime? fechaInicioPlan { get; set; }
        public DateTime? fechaUltimoPago { get; set; }
        public DateTime? fechaProximoPago { get; set; }
        public string observacionCartera { get; set; }
        public int periodicidadMeses { get; set; }
    }
}
