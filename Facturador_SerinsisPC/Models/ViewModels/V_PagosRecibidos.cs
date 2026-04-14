using System;

namespace Facturador_SerinsisPC.Models.ViewModels
{
    public class V_PagosRecibidos
    {
        public int id { get; set; }
        public DateTime fechaPago { get; set; }
        public string nombreComercial { get; set; }
        public string nombreRepresentate { get; set; }
        public string nombreMetodo { get; set; }
        public decimal valorRecibido { get; set; }
        public string numeroComprobante { get; set; }
        public string referenciaPago { get; set; }
        public string observacion { get; set; }
    }
}
