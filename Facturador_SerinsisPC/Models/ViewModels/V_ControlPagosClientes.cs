using System;

namespace Facturador_SerinsisPC.Models.ViewModels
{
    public class V_ControlPagosClientes
    {
        public int id { get; set; }
        public string nombreComercial { get; set; }
        public string nombreRepresentate { get; set; }
        public string celular { get; set; }
        public string nombrePlan { get; set; }
        public int periodicidadMeses { get; set; }
        public int? diaPago { get; set; }
        public DateTime? fechaInicioPlan { get; set; }
        public DateTime? fechaUltimoPago { get; set; }
        public DateTime? fechaProximoPago { get; set; }
        public decimal saldoPendienteTotal { get; set; }
        public DateTime? proximoVencimiento { get; set; }
    }
}
