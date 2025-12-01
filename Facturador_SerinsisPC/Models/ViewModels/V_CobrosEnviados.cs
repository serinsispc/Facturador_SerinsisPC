using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facturador_SerinsisPC.Models.ViewModels
{
    public class V_CobrosEnviados
    {
        public int id {  get; set; }
        public DateTime fechaCobro { get; set; }
        public int idCliente { get; set; }
        public string nombreComercial { get; set; }
        public string nombreRepresentate { get; set; }
        public string celular { get; set; }
        public int sedesCobradas { get; set; }
        public int mesesCobrados { get; set; }
        public decimal valotTotalCobrado { get; set; }
    }
}